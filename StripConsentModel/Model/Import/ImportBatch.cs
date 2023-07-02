using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StripConsentModel.Model.Import
{
    public class ImportBatch
    {
        public ImportBatch(string batchFolderPath)
        {
            BatchFolderPath = batchFolderPath;
        }

        public string BatchFolderPath { get; private set; }
        public List<ImportFile> Files { get; private set; } = new List<ImportFile>();

        public void Add(ImportFile file) => Files.Add(file);
        public void Remove(ImportFile file) => Files.Remove(file);

        //deprectated - alex
        //    private TrustInfo _trustInfo;

        //    public TrustInfo TrustInfo
        //    {
        //        get
        //        {
        //            if (_trustInfo == null)
        //            {
        //                _trustInfo = SearchForTrustInfo();
        //            }
        //            return _trustInfo;
        //        }
        //    }

        //    //private void RecalculateMismatchedIBDAuditCode

        //    private TrustInfo SearchForTrustInfo()
        //    {
        //        var FilesWithIBDAuditCode = Files.Where(f => !f.SpecificationFile.Name.Contains("provenance"));
        //        List<Record> AllRecords = FilesWithIBDAuditCode.SelectMany(f => f.Records).ToList();
        //        Func<List<Record>, string, IEnumerable<string>> GetFieldFromRecords = (Records, DataItemCode) =>
        //            Records
        //                .Select(r => r.GetValueByDataItemCode(DataItemCode))
        //                .Where(x => !string.IsNullOrEmpty(x));

        //        IEnumerable<string> IBDAuditCodes = GetFieldFromRecords(AllRecords, DataItemCodes.IBDAuditCode);

        //        var Groupings = IBDAuditCodes.GroupBy(x => x).Select(grouping => grouping.Key);

        //        if (Groupings.Count() > 1)
        //        {
        //            var MostCommonIBDAuditCode = Groupings.OrderByDescending(g => g.Length).FirstOrDefault();

        //            Func<List<Record>, string, IEnumerable<(Record record, string auditCode)>> GetFieldFromRecordsAndPreserveRecordReference = (Records, DataItemCode) =>
        //                Records
        //                    .Select(r => (record: r, auditCode: r.GetValueByDataItemCode(DataItemCode)))
        //                    .Where(x => !string.IsNullOrEmpty(x.auditCode));

        //            IEnumerable<(string auditCode, int lineNumber, string file)> IBDAuditCodeAndPositionInFile =
        //                GetFieldFromRecordsAndPreserveRecordReference(AllRecords, DataItemCodes.IBDAuditCode)
        //                .Select(x => (
        //                    auditCode: x.auditCode,
        //                    lineNumber: Array.IndexOf(x.record.OriginalFile.Records, x.record),
        //                    file: Path.GetFileName(x.record.OriginalFile.SpecificationFile.SimplifiedName)
        //                    ))
        //                .Where(composite => !composite.auditCode.Equals(MostCommonIBDAuditCode));

        //            var TrustFolderName = new DirectoryInfo(Files.First().Batch.BatchFolderPath).Name;

        //            throw new Exception($"Mismatched IBD Audit code in folder {TrustFolderName}\n Trust files contained the following IBD Audit codes\n" +
        //                string.Join("\n",
        //                IBDAuditCodeAndPositionInFile.Select(composite => $"{composite.auditCode} in {composite.file} at record index {composite.lineNumber}"
        //                )));
        //        }

        //        if (IBDAuditCodes.Count() == 0)
        //        {
        //            return new TrustInfo("");
        //        }
        //        else
        //        {
        //            return new TrustInfo(IBDAuditCodes.First());
        //        }

        //    }
    }

    //public static class ExtenstionMethods
    //{
    //    public static bool AnyExceptionsInList<T>(this IEnumerable<T> source) => source.Any(x => !x.Equals(source.FirstOrDefault()));
        
    //}

    //public class TrustInfo
    //{
    //    public string IBDAuditCode;

    //    public TrustInfo(string ibdAuditCode)
    //    {
    //        IBDAuditCode = ibdAuditCode;
    //    }
    //}
}
