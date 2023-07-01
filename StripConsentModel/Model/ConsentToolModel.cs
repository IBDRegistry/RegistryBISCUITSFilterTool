using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using StripConsentModel.Model.Output;
using StripConsentModel.Model.Import;

namespace StripV3Consent.Model
{
    public class ConsentToolModel
    {
        public readonly ImportBatchList ImportBatches = new ImportBatchList();

        public readonly ObservableRangeCollection<ImportFile> ImportFiles = new ObservableRangeCollection<ImportFile>();

        private RecordSet[] patients;
        public RecordSet[] Patients {
            get { return patients; }
            private set
            {
                patients = value;
                PatientsChanged.Invoke(this);
            }
        }
        public event ConsentToolModelEventHandler PatientsChanged;

        private OutputFile[] outputFiles;
        public OutputFile[] OutputFiles
        {
            get { return outputFiles; }
            private set
            {
                outputFiles = value;
                OutputFilesChanged.Invoke(this);
            }
        }
        public event ConsentToolModelEventHandler OutputFilesChanged;

        public delegate void ConsentToolModelEventHandler(ConsentToolModel sender);


        private bool enableNationalOptOut;
        public bool EnableNationalOptOut { get => enableNationalOptOut;
            set { 
                enableNationalOptOut = value;
                if (ImportFiles.Count > 0)
                    ProcessInputFiles();
            }
        }

        private bool enableConsentCheck;
        public bool EnableConsentCheck
        {
            get => enableConsentCheck;
            set
            {
                enableConsentCheck = value;
                if (ImportFiles.Count > 0)
                    ProcessInputFiles();
            }
        }

        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
        public event ProgressEventHandler Progress;

        private void InputFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action is NotifyCollectionChangedAction.Reset)
            {
                return;
            }

            ProcessInputFiles();
        }

        private void ProcessInputFiles()
        {
            IEnumerable<ImportFile> ValidFilesForImport = ImportFiles.Where(i => i.IsValid.ErrorLevel != ValidState.Error);

            IEnumerable<ImportFile> PatientLevelImportFiles = ValidFilesForImport.Where(i => i.SpecificationFile.IsPatientLevelFile == true);

            Progress?.Invoke(this, new ProgressEventArgs(new ConsentToolProgress(ConsentToolProgress.Stages.GroupingRecords)));

            IEnumerable<RecordSet> NewPatients = SplitInputFilesIntoRecordSets(PatientLevelImportFiles.ToList(), EnableNationalOptOut);

            Patients = NewPatients.ToArray();

            Progress?.Invoke(this, new ProgressEventArgs(new ConsentToolProgress(ConsentToolProgress.Stages.SplittingBackUp)));

            IEnumerable<RepackingOutputFile> NewOutputFiles = SplitBackUpIntoFiles(patients, EnableConsentCheck);    //slow

            Progress?.Invoke(this, new ProgressEventArgs(new ConsentToolProgress(ConsentToolProgress.Stages.AddingToOutput)));

            IEnumerable<ImportFile> NonPatientLevelImportFiles = ValidFilesForImport.Except(PatientLevelImportFiles);
            IEnumerable<DirectOutputFile> DirectOutputFiles = NonPatientLevelImportFiles.Select(importFile => new DirectOutputFile
             (
                 file: importFile,
                 contentToOutput: importFile.FileContents
             )
            ).ToArray();

            IEnumerable<OutputFile> AllOutputFiles = NewOutputFiles.Union<OutputFile>(DirectOutputFiles);
            OutputFiles = AllOutputFiles.ToArray();

            Progress?.Invoke(this, new ProgressEventArgs(new ConsentToolProgress(ConsentToolProgress.Stages.Finished))); //107s
        }



        public ConsentToolModel()
        {
            ImportFiles.CollectionChanged += InputFiles_CollectionChanged;
            ImportFiles.CollectionChanged += ImportBatches.ImportFiles_CollectionChanged;
        }


        /// <summary>
        /// Splits ImportFile objects into Records, and then groups them by identifier to make RecordSets
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static IEnumerable<RecordSet> SplitInputFilesIntoRecordSets(IEnumerable<ImportFile> Files, bool EnableNationalOptOut)
        {
            List<Record> AllRecords = Files
                                            .Select(File => File.Records)         //Transform DataFile into string[][] resulting in IEnumerable<string[][]>
                                                .SelectMany<IEnumerable<Record>, Record>(x => x)                //Flatten into IEnumerable<Record>
                                                .ToList<Record>();                                            //Cast to List<Record>
                                                                                                              //Task<string[][]>[] AllTasks = Files.Select(async File => (await File.SplitInto2DArray()).Content).ToArray();

            //Group records by identifiers (NHS Number & Date of Birth)
            IEnumerable<RecordSet> GroupedRecords = AllRecords.GroupBy
                                                                        <Record, string, RecordSet>(//Take in Record, group by String, Output RecordSet
                                                                        r => r.CompositeIdentifier,
                                                                        (NHSNumber, RecordsIEnumerable) => new RecordSet(ShouldNationalOptOutBeChecked: EnableNationalOptOut)
                                                                        {
                                                                            Records = RecordsIEnumerable.ToList<Record>()
                                                                        }
                                                                        );

            return GroupedRecords;
        }
        public class RecordWithOriginalSet
        {
            public readonly Record Record;
            public readonly RecordSet OriginalSet;

            public RecordWithOriginalSet(Record record, RecordSet originalSet)
            {
                Record = record;
                OriginalSet = originalSet;
            }
        }
        private static IEnumerable<IEnumerable<RecordWithOriginalSet>> FlattenAndGroupRecordsBySpecificationFiles(IEnumerable<RecordSet> RecordSets) =>
            RecordSets
                .AsParallel()
                .SelectMany(rs => rs.Records.Select(r => new RecordWithOriginalSet(record: r, originalSet: rs)))   //Flatten, but preserve original Recordset for enhancement later
                .AsParallel()

                .Where(r => r.Record.OriginalFile.SpecificationFile.IsRegistryFile == true)       //Filter for output

                .GroupBy(
                        r => r.Record.OriginalFile.SpecificationFile,    //Group by original file
                        (OriginalFile, RecordsIEnumerable) => RecordsIEnumerable    //Output groupings as IEnumerable<Record>
                        );
        

        private static IEnumerable<RepackingOutputFile> SplitBackUpIntoFiles(IEnumerable<RecordSet> RecordSets, bool CheckConsents)
        {
            var ConsentAlwaysValid = !CheckConsents;
            IEnumerable<IEnumerable<RecordWithOriginalSet>> AllConsentedRecordsGroupedByFile = FlattenAndGroupRecordsBySpecificationFiles(RecordSets.Where(RS => RS.IsConsentValid == true || ConsentAlwaysValid));

            //Have to do ToList in order to use Find() later on which isn't present in IEnumerable<T>
            List<IEnumerable<RecordWithOriginalSet>> AllRecordsGroupedByFile = FlattenAndGroupRecordsBySpecificationFiles(RecordSets).ToList(); //slow 121s
            Func<IEnumerable<RecordWithOriginalSet>, IEnumerable<Record>> GetOriginalRecords = (OriginalRecords) =>
                AllRecordsGroupedByFile.Find(
                    FileAllRecords => FileAllRecords.First().Record.OriginalFile.SpecificationFile == OriginalRecords.First().Record.OriginalFile.SpecificationFile) //Find the full set (consented and non-consented) of records by looking through AllRecordsGroupedByFile for one with the same OriginalFile attribute
                .Select(x => x.Record);


            IEnumerable<RepackingOutputFile> Files =
                AllConsentedRecordsGroupedByFile.Select(FileOutputRecords => {
                    const string PatientFilename = "patient";
                    if (FileOutputRecords.First().Record.OriginalFile.SpecificationFile.SimplifiedName.Contains(PatientFilename))
                    {
                        return new PatientEnhancedOutputFile(
                            file: FileOutputRecords.First().Record.OriginalFile,
                            outputRecords: FileOutputRecords,    ///Match each set of records to a new OutputFile object
                            allRecordsOriginallyInFile: GetOriginalRecords(FileOutputRecords)
                            );
                    } else
                    {
                        return new StandardEnhancedOutputFile(
                            file: FileOutputRecords.First().Record.OriginalFile,
                            outputRecords: FileOutputRecords,    ///Match each set of records to a new OutputFile object
                            allRecordsOriginallyInFile: GetOriginalRecords(FileOutputRecords));
                    }
                    
                    });
            return Files;
        }

    }

    public class ConsentToolProgress
    {
        public enum Stages
        {
            GroupingRecords,
            SplittingBackUp,
            AddingToOutput,
            Finished
        }
        public Stages stage;

        public bool Finished = false;

        public ConsentToolProgress(Stages stage)
        {
            this.stage = stage;
        }
        public string StageToString()
        {
            switch (stage)
            {
                case Stages.GroupingRecords:
                    return "Grouping records into patients";
                case Stages.SplittingBackUp:
                    return "Splitting patients back into files";
                case Stages.AddingToOutput:
                    return "Adding to output";
                case Stages.Finished:
                    return "Finished";
                default:
                    return "Working";
            }
        }
        
    }

    public class ProgressEventArgs : EventArgs
    {
        public ConsentToolProgress ProgressInfo;

        public ProgressEventArgs(ConsentToolProgress progressInfo)
        {
            ProgressInfo = progressInfo;
        }
    }
}
