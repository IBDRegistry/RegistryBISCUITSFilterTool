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
                            writer.Write(Properties.Resources.SiteList);
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

        public static string GetAuditCodeFromReportingName(string ReportingName)
        {
            var FoundEntry = LookupEntries.FirstOrDefault(x => x.ReportingName.Equals(ReportingName));

            return FoundEntry?.ReportingCode;
        }

        public static SiteLookupEntry GetLookupEntryFromAuditCode(string IBDAuditCode)
        {
            var FoundEntry = LookupEntries.FirstOrDefault(x => x.SiteCode.Equals(IBDAuditCode));

            return FoundEntry;
        }
    }
}