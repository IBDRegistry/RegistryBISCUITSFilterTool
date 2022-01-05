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

        public RecordSetGrouping(ImportFile[] FileContents)
        {
            LoadFiles(FileContents);
        }

        public RecordSetGrouping(List<RecordSet> ListOfRecordSetsToUse)
        {
            RecordSets = ListOfRecordSetsToUse;
        }

        private void LoadFiles(ImportFile[] Files)
        {
            List<Record> AllRecords = Files
                                            .Select(File => File.SplitInto2DArray().Content         //Transform DataFile into string[][] resulting in IEnumerable<string[][]>
                                               .Select(Row => new Record(Row, File)))                             //Transform each string[] into Record resulting in IEnumerable<IEnumerable<Record>>
                                                  .SelectMany<IEnumerable<Record>, Record>(x => x)                //Flatten into IEnumerable<Record>
                                                    .ToList<Record>();                                            //Cast to List<Record>
                                                                                                                  //Task<string[][]>[] AllTasks = Files.Select(async File => (await File.SplitInto2DArray()).Content).ToArray();

            //Group records by identifiers (NHS Number & Date of Birth)
            RecordSets = AllRecords.GroupBy
                                        <Record, string, RecordSet>(//Take in Record, group by String, Output RecordSet
                                        r => r.CompositeIdentifier,
                                        (NHSNumber, RecordsIEnumerable) => new RecordSet()
                                        {
                                            Records = RecordsIEnumerable.ToList<Record>()
                                        }
                                        ).ToList<RecordSet>();


        }

        public LoadedFile[] SplitBackUpIntoFiles()
        {
            IEnumerable<Record> AllRecords = RecordSets.Select(rs => rs.Records).SelectMany<IEnumerable<Record>, Record>(x => x);


            IEnumerable<Record[]> RecordsGroupedByOriginalFiles = AllRecords.GroupBy
                                                                                <Record, DataFile, Record[]>(
                                                                                r => r.OriginalFile,
                                                                                (OriginalFile, RecordsIEnumerable) => RecordsIEnumerable.ToArray()
                                                                                );


            var Files = RecordsGroupedByOriginalFiles.Select(
                                                        RecordArray => new LoadedFile
                                                        (
                                                            content: RecordArray.Select(r => r.DataRecord).ToArray(),
                                                            file: RecordArray.First().OriginalFile.File
                                                        ));



            return Files.ToArray();

        }

        public static explicit operator LoadedFile[](RecordSetGrouping RSG) => RSG.SplitBackUpIntoFiles();

        public static explicit operator RecordSetGrouping(List<RecordSet> ListOfRecordSets) => new RecordSetGrouping(ListOfRecordSets);


    }
}
