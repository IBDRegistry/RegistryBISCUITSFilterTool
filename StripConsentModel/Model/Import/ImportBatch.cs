﻿using StripV3Consent.Model;
using System;
using System.Collections.Generic;
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

        private TrustInfo _trustInfo;

        public TrustInfo TrustInfo {  
            get
            {
                if (_trustInfo == null)
                {
                    _trustInfo = SearchForTrustInfo();
                }
                return _trustInfo;
            } 
        }

        private TrustInfo SearchForTrustInfo()
        {
            List<Record> AllRecords = Files.SelectMany(f => f.Records).ToList();
            Func<List<Record>, string, IEnumerable<string>> GetFieldFromRecords = (Records, DataItemCode) =>
                Records
                    .Select(r => r.GetValueByDataItemCode(DataItemCode))
                    .Where(x => !string.IsNullOrEmpty(x));

            IEnumerable<string> IBDAuditCodes = GetFieldFromRecords(AllRecords, DataItemCodes.IBDAuditCode);

            var Groupings = IBDAuditCodes.GroupBy(x => x).Select(grouping => grouping.Key);

            if (Groupings.Count() > 1)
            {
                throw new Exception("Mismatched IBD Audit code, Trust files contained the following IBD Audit codes\n" + 
                    string.Join(" ", Groupings));
            }

            if (IBDAuditCodes.Count() == 0)
            {
                return new TrustInfo("");
            }
            else
            {
                return new TrustInfo(IBDAuditCodes.First());
            }
            
        }
    }

    public static class ExtenstionMethods
    {
        public static bool AnyExceptionsInList<T>(this IEnumerable<T> source) => source.Any(x => !x.Equals(source.FirstOrDefault()));
        
    }

    public class TrustInfo
    {
        public string IBDAuditCode;

        public TrustInfo(string ibdAuditCode)
        {
            IBDAuditCode = ibdAuditCode;
        }
    }
}
