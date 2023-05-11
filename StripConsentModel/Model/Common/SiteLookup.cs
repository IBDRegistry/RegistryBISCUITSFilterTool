using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StripConsentModel.SiteLookup
{
    public static class SiteLookup
    {
        private static List<SiteLookupEntry> _lookupEntries = null;

        private static List<SiteLookupEntry> LookupEntries {
            get {
                if (_lookupEntries == null)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(Properties.Resources.SiteMaster);
                            writer.Flush();
                            stream.Position = 0;

                            using (var reader = new StreamReader(stream))
                            {
                                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                                {
                                    HasHeaderRecord = true,
                                    Delimiter = ","
                                };
                                using (var csv = new CsvReader(reader, config))
                                {
                                    _lookupEntries = csv.GetRecords<SiteLookupEntry>().ToList();
                                }
                            }
                        }
                    }
                }
                return _lookupEntries;
            }
        }

        public static SiteLookupEntry GetLookupEntryFromAuditCode(string IBDAuditCode)
        {
            var FoundEntry = LookupEntries.FirstOrDefault(x => x.IBDAuditCode.Equals(IBDAuditCode));
            if (FoundEntry == null)
            {
                throw new System.Exception($"Site with IBD Audit Code {IBDAuditCode} was not in lookup table");
            }
            return FoundEntry;
        }
    }
}