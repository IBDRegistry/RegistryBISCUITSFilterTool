using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public static class DataItemCodes
    {
        public static readonly string NHSNumber = "IBD01";
        public static readonly string NationalOptOut = "NHS01";
        public static readonly string DateOfBirth = "IBD02";
        public static readonly string LocalUnitCode = "IBD13";
        public static readonly string IBDAuditCode = "IBD14";
        public static readonly string Postcode = "IBD04";
        public static readonly string DateOfDeath = "IBD06";
        public static readonly string IBDDiagnosis = "IBD09";
        public static readonly string DateOfDiagnosis = "IBD08";

        public static class Consent
        {
            public static readonly string Registry = "IBD16";
            public static readonly string DateConsentLastRecorded = "IBD15";
            public static readonly string Linkage = "IBD17";
            public static readonly string Research = "IBD18";
            public static readonly string ContactFutureResearch = "IBD19";
            public static readonly string Companies = "IBD19A";
            public static readonly string Version = "IBD338";

        }
    }
}
