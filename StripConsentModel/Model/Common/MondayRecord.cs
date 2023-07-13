using CsvHelper.Configuration.Attributes;

namespace StripConsentModel.SiteLookup
{
    public class MondayRecord
    {
        [Index(0)]
        public string Name { get; set; }
        [Index(1)]
        public string Subitems { get; set; }
        [Index(2)]
        public string Version { get; set; }
        [Index(3)]
        public string SiteName { get; set; }
        [Index(4)]
        public string IBDAuditCode { get; set; }
        [Index(5)]
        public string TrustHealthBoard { get; set; }
        [Index(6)]
        public string TrustHealthBoardIBDCode { get; set; }
        [Index(7)]
        public string IBDService { get; set; }
        [Index(8)]
        public string ServiceIBDCode { get; set; }
        [Index(9)]
        public string Source { get; set; }
        [Index(10)]
        public string SourceSM { get; set; }
        [Index(11)]
        public string AutoNumber { get; set; }
        [Index(12)]
        public string AdultPaeds { get; set; }
        [Index(13)]
        public string Country { get; set; }
        [Index(14)]
        public string ClinicalSystem { get; set; }
        [Index(15)]
        public string WebToolUse { get; set; }
        [Index(16)]
        public string Grant { get; set; }
        [Index(17)]
        public string NDA { get; set; }
        [Index(18)]
        public string IBDSubSiteCode { get; set; }
        [Index(19)]
        public string ODScode { get; set; }
        [Index(20)]
        public string ODSLevel { get; set; }
        [Index(21)]
        public string Registry2020 { get; set; }
        [Index(22)]
        public string Agrs2020 { get; set; }
        [Index(23)]
        public string QA202122 { get; set; }
        [Index(24)]
        public string Postcode { get; set; }
        [Index(25)]
        public string Financebreakdown2021	 { get; set; }
        [Index(26)]
        public string QualityAccounts { get; set; }
        [Index(27)]
        public string QA201920 { get; set; }


    }
}

