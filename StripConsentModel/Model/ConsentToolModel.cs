using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Text;

namespace StripV3Consent.Model
{
    public class ConsentToolModel
    {
        public readonly ObservableRangeCollection<ImportFile> InputFiles = new ObservableRangeCollection<ImportFile>();

        public event NotifyCollectionChangedEventHandler InputFilesChanged;

        private static bool enableNationalOptOut;
        public static bool EnableNationalOptOut { get => enableNationalOptOut;
            set { 
                enableNationalOptOut = value;
                EnableNationalOptOutChanged.Invoke();
            }
        }
        public static event Action EnableNationalOptOutChanged;

        private void InputFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Patients.Clear();
            IEnumerable<ImportFile> ValidFilesForImport = InputFiles.Where(i => i.IsValid.ValidState != ValidState.Error);

            IEnumerable<ImportFile> NonPatientLevelFiles = ValidFilesForImport.Where(i => i.SpecificationFile.IsPatientLevelFile == false);
            IEnumerable<ImportFile> ValidFilesForProcessing = ValidFilesForImport.Except(NonPatientLevelFiles);

            IEnumerable<RecordSet> NewPatients = SplitInputFilesIntoRecordSets(ValidFilesForProcessing.ToList());
            Patients.AddRange(NewPatients);

            OutputFiles.Clear();
            IEnumerable<OutputFile> NewOutputFiles = SplitBackUpIntoFiles(Patients);
            OutputFiles.AddRange(NewOutputFiles);
            OutputFiles.AddRange(NonPatientLevelFiles.Select(i => new OutputFile(i)));
        }

        public readonly ObservableRangeCollection<RecordSet> Patients = new ObservableRangeCollection<RecordSet>();
        
        public readonly ObservableRangeCollection<OutputFile> OutputFiles = new ObservableRangeCollection<OutputFile>();

        

        public ConsentToolModel()
        {
            InputFiles.CollectionChanged += InputFiles_SubCollectionChanged;
            InputFilesChanged += InputFiles_CollectionChanged;
            EnableNationalOptOutChanged += ConsentToolModel_EnableNationalOptOutChanged;
        }

        private void InputFiles_SubCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InputFilesChanged?.Invoke(sender, e);
        }

        private void ConsentToolModel_EnableNationalOptOutChanged()
        {
            //Reload other file lists to take into account NOO (or not)
            InputFilesChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Splits ImportFile objects into Records, and then groups them by identifier to make RecordSets
        /// </summary>
        /// <param name="Files"></param>
        /// <returns></returns>
        public static IEnumerable<RecordSet> SplitInputFilesIntoRecordSets(IEnumerable<ImportFile> Files)
        {
            List<Record> AllRecords = Files
                                            .Select(File => File.SplitInto2DArray().Content         //Transform DataFile into string[][] resulting in IEnumerable<string[][]>
                                               .Select(Row => new Record(Row, File)))                             //Transform each string[] into Record resulting in IEnumerable<IEnumerable<Record>>
                                                .SelectMany<IEnumerable<Record>, Record>(x => x)                //Flatten into IEnumerable<Record>
                                                .ToList<Record>();                                            //Cast to List<Record>
                                                                                                              //Task<string[][]>[] AllTasks = Files.Select(async File => (await File.SplitInto2DArray()).Content).ToArray();

            //Group records by identifiers (NHS Number & Date of Birth)
            IEnumerable<RecordSet> GroupedRecords = AllRecords.GroupBy
                                                                        <Record, string, RecordSet>(//Take in Record, group by String, Output RecordSet
                                                                        r => r.CompositeIdentifier,
                                                                        (NHSNumber, RecordsIEnumerable) => new RecordSet()
                                                                        {
                                                                            Records = RecordsIEnumerable.ToList<Record>()
                                                                        }
                                                                        );

            return GroupedRecords;
        }

        private IEnumerable<IEnumerable<Record>> FlattenAndGroupRecordsBySpecificationFiles(IEnumerable<RecordSet> RecordSets)
        {
            IEnumerable<Record> RecordsFlattened = RecordSets.Select(rs => rs.Records).SelectMany<IEnumerable<Record>, Record>(x => x);

            IEnumerable<Record> OutputRecords = RecordsFlattened.Where(r => r.OriginalFile.SpecificationFile.IsRegistryFile == true);

            IEnumerable<IEnumerable<Record>> RecordsGroupedByOriginalFiles = OutputRecords.GroupBy
                                                                                <Record, Specification.File, IEnumerable<Record>>(
                                                                                r => r.OriginalFile.SpecificationFile,    //Group by original file
                                                                                (OriginalFile, RecordsIEnumerable) => RecordsIEnumerable    //Output groupings as IEnumerable<Record>
                                                                                );
            return RecordsGroupedByOriginalFiles;
        }

        private IEnumerable<OutputFile> SplitBackUpIntoFiles(IEnumerable<RecordSet> RecordSets)
        {
            IEnumerable<IEnumerable<Record>> AllConsentedRecordsGroupedByFile = FlattenAndGroupRecordsBySpecificationFiles(RecordSets.Where(RS => RS.IsConsentValid == true));

            //Have to do ToList in order to use Find() later on which isn't present in IEnumerable<T>
            List<IEnumerable<Record>> AllRecordsGroupedByFile = FlattenAndGroupRecordsBySpecificationFiles(RecordSets).ToList<IEnumerable<Record>>();


            IEnumerable<OutputFile> Files = AllConsentedRecordsGroupedByFile.Select(FileOutputRecords => new OutputFile(FileOutputRecords.First().OriginalFile) //Match each set of records to a new OutputFile object
                                                                {
                                                                    OutputRecords = FileOutputRecords,    //Make the OutputRecords the current set of (consented) records
                                                                    AllRecordsOriginallyInFile = AllRecordsGroupedByFile.Find(FileAllRecords => FileAllRecords.First().OriginalFile.SpecificationFile == FileOutputRecords.First().OriginalFile.SpecificationFile)
                                                                    //Find the full set (consented and non-consented) of records by looking through AllRecordsGroupedByFile for one with the same OriginalFile attribute
                                                                }
                                                            );
            return Files;
        }

    }
}
