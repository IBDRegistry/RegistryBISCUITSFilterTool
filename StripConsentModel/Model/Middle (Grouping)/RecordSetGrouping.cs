using System;
using System.Collections.Generic;
using System.Linq;

namespace StripV3Consent.Model
{
    /// <summary>
    /// All of the recordsets from all of the files
    /// </summary>
    public class RecordSetGrouping
    {
        public List<RecordSet> RecordSets;

        //public RecordSetGrouping(ImportFile[] FileContents)
        //{
        //    LoadFiles(FileContents);
        //}

        //public RecordSetGrouping(List<RecordSet> ListOfRecordSetsToUse)
        //{
        //    RecordSets = ListOfRecordSetsToUse;
        //}

        //private void LoadFiles(ImportFile[] Files)
        //{
        //    List<Record> AllRecords = Files
        //                                    .Select(File => File.SplitInto2DArray().Content         //Transform DataFile into string[][] resulting in IEnumerable<string[][]>
        //                                       .Select(Row => new Record(Row, File)))                             //Transform each string[] into Record resulting in IEnumerable<IEnumerable<Record>>
        //                                        .SelectMany<IEnumerable<Record>, Record>(x => x)                //Flatten into IEnumerable<Record>
        //                                        .ToList<Record>();                                            //Cast to List<Record>
        //                                                                                                          //Task<string[][]>[] AllTasks = Files.Select(async File => (await File.SplitInto2DArray()).Content).ToArray();

        //    //Group records by identifiers (NHS Number & Date of Birth)
        //    RecordSets = AllRecords.GroupBy
        //                                <Record, string, RecordSet>(//Take in Record, group by String, Output RecordSet
        //                                r => r.CompositeIdentifier,
        //                                (NHSNumber, RecordsIEnumerable) => new RecordSet()
        //                                {
        //                                    Records = RecordsIEnumerable.ToList<Record>()
        //                                }
        //                                ).ToList<RecordSet>();


        //}

        //private IEnumerable<IEnumerable<Record>> FlattenAndGroupRecordsByOriginalFiles(List<RecordSet> RecordSets)
        //{
        //    IEnumerable<Record> RecordsFlattened = RecordSets.Select(rs => rs.Records).SelectMany<IEnumerable<Record>, Record>(x => x);

        //    IEnumerable<Record> OutputRecords = RecordsFlattened.Where(r => r.OriginalFile.SpecificationFile.IsRegistryFile == true);

        //    IEnumerable<IEnumerable<Record>> RecordsGroupedByOriginalFiles = OutputRecords.GroupBy
        //                                                                        <Record, DataFile, IEnumerable<Record>>(
        //                                                                        r => r.OriginalFile,    //Group by original file
        //                                                                        (OriginalFile, RecordsIEnumerable) => RecordsIEnumerable    //Output groupings as IEnumerable<Record>
        //                                                                        );
        //    return RecordsGroupedByOriginalFiles;
        //}

        //private OutputFile[] SplitBackUpIntoFiles(List<RecordSet> RecordSets)
        //{
        //    IEnumerable<IEnumerable<Record>> OutputRecords = FlattenAndGroupRecordsByOriginalFiles(RecordSets);

        //    List<IEnumerable<Record>> AllRecords = FlattenAndGroupRecordsByOriginalFiles(this.RecordSets).ToList<IEnumerable<Record>>();
            

        //    IEnumerable<OutputFile> Files = OutputRecords.Select(
        //                                                                RecordsInOutputFile => new OutputFile(RecordsInOutputFile.First().OriginalFile) 
        //                                                                {
        //                                                                    OutputRecords = RecordsInOutputFile,
        //                                                                    AllRecordsOriginallyInFile = AllRecords.Find(RecordIEnumerable => RecordIEnumerable.First().OriginalFile == RecordsInOutputFile.First().OriginalFile)
        //                                                                }
        //                                                                );



        //    return Files.ToArray();
        //}
        //public OutputFile[] SplitBackUpIntoFiles()
        //{
        //    return SplitBackUpIntoFiles(this.RecordSets);

        //}

        //public OutputFile[] SplitBackUpIntoFiles(Func<RecordSet, bool> predicate)
        //{
        //    return SplitBackUpIntoFiles(this.RecordSets.Where(predicate).ToList<RecordSet>());
        //}

        //public static explicit operator OutputFile[](RecordSetGrouping RSG) => RSG.SplitBackUpIntoFiles();

        //public static explicit operator RecordSetGrouping(List<RecordSet> ListOfRecordSets) => new RecordSetGrouping(ListOfRecordSets);


    }
}
