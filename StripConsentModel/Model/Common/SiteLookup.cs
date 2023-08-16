using CsvHelper;
using CsvHelper.Configuration;
using StripConsentModel.Model.Common;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace StripConsentModel
{
    public static class SiteLookup
    {
        private static List<ILookupEntry> _lookupEntries = null;

        private static List<ILookupEntry> LookupEntries {
            get {
                if (_lookupEntries == null)
                {
                    _lookupEntries = LoadLookupEntries().ToList();
                }
                return _lookupEntries;
            }
        }

        private static AdultPaed ParseAdultPaed(string sourceValue)
        {
            if (sourceValue.Equals("Adult"))
            {
                return AdultPaed.Adult;
            }
            else if (sourceValue.Equals("Paediatric"))
            {
                return AdultPaed.Paediatric;
            } else
            {
                throw new System.Exception("Site in site master was neither adult nor paediatric");
            }
        }

        private static IEnumerable<ILookupEntry> LoadLookupEntries()
        {
            var MondayRecords = LoadMondayRecords().ToList();   //prevent multiple enumeration

            var SiteRecords = MondayRecords
                .Where(x => !string.IsNullOrEmpty(x.IBDAuditCode))
                .Select(x => new SiteLookupEntry(
                ibdAuditCode: x.IBDAuditCode,
                name: x.Name,
                adultPaeds: ParseAdultPaed(x.AdultPaeds)
                ));

            var TrustRecords = MondayRecords
                                    .GroupBy(mondayRecord => mondayRecord.TrustHealthBoardIBDCode)
                                    .Select(grouping => new TrustLookupEntry(
                                        ibdAuditCode: grouping.Key,
                                        name: MondayRecords.FirstOrDefault(source => source.TrustHealthBoardIBDCode == grouping.Key).TrustHealthBoard,
                                        sites: SiteRecords.Where(site => grouping.Select(group => group.IBDAuditCode).Contains(site.IBDAuditCode)).ToList()));

            var ServiceRecords = MondayRecords
                        .GroupBy(mondayRecord => mondayRecord.ServiceIBDCode)
                        .Select(grouping => new ServiceLookupEntry(
                            ibdAuditCode: grouping.Key,
                            name: MondayRecords.FirstOrDefault(source => source.ServiceIBDCode == grouping.Key).IBDService,
                            sites: SiteRecords.Where(site => grouping.Select(group => group.IBDAuditCode).Contains(site.IBDAuditCode)).ToList()));


            return ((IEnumerable<ILookupEntry>)SiteRecords).Concat(TrustRecords).Concat(ServiceRecords);
        }
        private static IEnumerable<MondayRecord> LoadMondayRecords()
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
                            return csv.GetRecords<MondayRecord>().ToList();
                        }
                    }
                }
            }
        }

        public static ILookupEntry GetLookupEntryFromAuditCode(string IBDAuditCode)
        {
            string CleanedIBDAuditCode = IBDAuditCode.Replace(" ", "");
            var FoundEntry = LookupEntries.FirstOrDefault(x => x.IBDAuditCode == CleanedIBDAuditCode);

            return FoundEntry;
        }
    }
}