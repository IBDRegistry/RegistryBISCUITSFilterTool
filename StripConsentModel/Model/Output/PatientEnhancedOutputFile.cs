﻿using StripConsentModel.SiteLookup;
using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static StripV3Consent.Model.ConsentToolModel;

namespace StripConsentModel.Model.Output
{
    class PatientEnhancedOutputFile : StandardEnhancedOutputFile
    {
        public PatientEnhancedOutputFile(DataFile file, IEnumerable<RecordWithOriginalSet> outputRecords, IEnumerable<Record> allRecordsOriginallyInFile) : base(file, outputRecords, allRecordsOriginallyInFile)
        {
        }


        protected override Record EnhanceRecord(RecordWithOriginalSet PatientRecordAndRecordSet)
        {
            Record PatientRecord = PatientRecordAndRecordSet.Record;

            if (!PatientRecord.OriginalFile.SpecificationFile.SimplifiedName.Contains("patient"))
            {
                throw new Exception("Patient enhanced record not from patient file");
            }

            DateTime DateOfBirth;
            var SuccessfulDoBParse = DateTime.TryParse(PatientRecord.GetValueByDataItemCode(DataItemCodes.DateOfBirth), out DateOfBirth);
            //IBDR_MonthofBirth
            string IBDR_MonthOfBirth = "";
            if (SuccessfulDoBParse)
            {
                IBDR_MonthOfBirth = DateOfBirth.ToString("MM");
            }

            //IBDR_YearOfBirth
            string IBDR_YearOfBirth = "";
            if (SuccessfulDoBParse)
            {
                IBDR_YearOfBirth = DateOfBirth.ToString("yyyy");
            }

            //IBDR_UKCountry    -   looks hard, leave for later
            string Postcode = PatientRecord.GetValueByDataItemCode(DataItemCodes.Postcode);
            string IBDR_UKCountry = "";


            //IBDR_LSOA -   also hard
            string IBDR_LSOA = "";

            DateTime DateOfDeath;
            var SuccessfulDoDParse = DateTime.TryParse(PatientRecord.GetValueByDataItemCode(DataItemCodes.DateOfDeath), out DateOfDeath);
            //IBDR_MonthofDeath
            string IBDR_MonthofDeath = "";
            if (SuccessfulDoDParse)
            {
                IBDR_MonthofDeath = DateOfDeath.ToString("MM");
            }

            //IBDR_YearofDeath
            string IBDR_YearofDeath = "";
            if (SuccessfulDoDParse)
            {
                IBDR_YearofDeath = DateOfDeath.ToString("yyyy");
            }


            //IBDR_CreatedDateTime
            //IBDR_CreatedByIBDAuditCode
            //IBDR_UpdatedDateTime
            //IBDR_UpdatedByIBDAuditCode
            //IBDR_HASH IBDR_Source
            //IBDR_Submission


            //IBDR_DerivedAge
            string IBDR_DerivedAge = "";
            int Years(DateTime start, DateTime end)
            {
                return (end.Year - start.Year - 1) +
                    (((end.Month > start.Month) ||
                    ((end.Month == start.Month) && (end.Day >= start.Day))) ? 1 : 0);
            }
            if (SuccessfulDoBParse)
            {
                IBDR_DerivedAge = Years(DateOfBirth, DateTime.Now).ToString();
            }

            //IBDR_CurrentDiagnosis
            var AllDiagnosises = PatientRecordAndRecordSet.OriginalSet.GetRecordsWithDataItemCode(DataItemCodes.IBDDiagnosis);
            var LatestDiagnosis = AllDiagnosises.OrderBy(r => r.GetValueByDataItemCode(DataItemCodes.DateOfDiagnosis)).FirstOrDefault();
            var IBDR_CurrentDiagnosis = "";
            if (LatestDiagnosis != null)
            {
                IBDR_CurrentDiagnosis = LatestDiagnosis.GetValueByDataItemCode(DataItemCodes.IBDDiagnosis);
            }

            //IBDR_DiseaseDuration
            string IBDR_DiseaseDuration = "";
            string DateOfDiagnosis = PatientRecordAndRecordSet.OriginalSet
                .GetRecordsWithDataItemCode(DataItemCodes.DateOfDiagnosis)
                .OrderBy(r => r.GetValueByDataItemCode(DataItemCodes.DateOfDiagnosis))
                .FirstOrDefault()
                ?.GetValueByDataItemCode(DataItemCodes.DateOfDiagnosis);
            if (SuccessfulDoBParse && DateOfDiagnosis != null)
            {
                IBDR_DiseaseDuration = Years(DateOfBirth, DateTime.Parse(DateOfDiagnosis)).ToString();
            }

            //IBDR_ReportGroup
            var IBDRAuditCode = PatientRecord.OriginalFile.Batch.TrustInfo.IBDAuditCode;
            var SiteType = SiteLookup.SiteLookup.GetLookupEntryFromAuditCode(IBDRAuditCode)?.SiteType;
            var Age = int.Parse(IBDR_DerivedAge);
            string IBDR_ReportGroup = "";
            //this piece of garbage branching is derived from the original SQL report driver (below)
            //(case WHEN(("pat"."IBDR_DerivedAge" > 15) AND("ibdr_reference"."sites"."IBDR_SiteType" <> 'Paediatric')) THEN 'Adult' 
            //WHEN(("pat"."IBDR_DerivedAge" <= 15) AND("ibdr_reference"."sites"."IBDR_SiteType" = 'Mixed')) THEN 'Paediatric'
            //WHEN("ibdr_reference"."sites"."IBDR_SiteType" = 'Paediatric') THEN 'Paediatric'
            //WHEN(("pat"."IBDR_DerivedAge" <= 15) AND("ibdr_reference"."sites"."IBDR_SiteType" = 'Adult')) THEN 'Adult' else '' end) AS "IBDR_ReportGroup"
            if (Age > 15 & !SiteType.Equals("Paediatric"))
            {
                IBDR_ReportGroup = "Adult";
            }
            else if (Age <= 15 & SiteType.Equals("Mixed"))
            {
                IBDR_ReportGroup = "Paediatric";
            } else if (SiteType.Equals("Paediatric")) { 
                IBDR_ReportGroup = "Paediatric";
            } else if (Age <= 15 & SiteType.Equals("Adult")){
                IBDR_ReportGroup = "Adult";
            }

            //IBDR_SiteType
            var IBDR_SiteType = SiteType != null ? SiteType : "";

            //IBDR_ConsentGroup


            //IBDR_TableGeneratedDateTime
            var IBDR_TableGeneratedDateTime = "";

            var record = PatientRecordAndRecordSet.Record;
            string[] ToAppend = new string[] {
                IBDR_MonthOfBirth,
                IBDR_YearOfBirth,
                IBDR_UKCountry,
                IBDR_LSOA,
                IBDR_MonthofDeath,
                IBDR_YearofDeath,
                IBDR_CreatedDateTime(record),
                IBDR_CreatedByIBDAuditCode(record),
                IBDR_UpdatedDateTime(record),
                IBDR_UpdatedByIBDAuditCode(record),
                IBDR_Hash(record),
                IBDR_Source,
                IBDR_Submission(record),
                IBDR_DerivedAge,
                IBDR_CurrentDiagnosis,
                IBDR_DiseaseDuration,
                IBDR_ReportGroup,
                IBDR_SiteType,
                IBDR_ConsentGroup(PatientRecordAndRecordSet.OriginalSet),
                IBDR_TableGeneratedDateTime
            };

            return new Record(record.DataRecord.AppendArray(ToAppend), PatientRecordAndRecordSet.Record.OriginalFile);
        }

        private string IBDR_ConsentGroup(RecordSet rs)
        {
            List<string> ConsentStrings = new List<string>();
            ConsentState RegistryConsent = ConsentValidation.IsConsentValid(recordset: rs, EnableNationalOptOut: false);
            if (RegistryConsent is S251)
            {
                ConsentStrings.Add("Y - Registry via s251");
            }
            else if (RegistryConsent is V4Consent)
            {
                ConsentStrings.Add("Y - Registry via Consent");
            }

            if (rs.GetFieldValue(DataItemCodes.Consent.Linkage).Equals("Y"))
            {
                ConsentStrings.Add("Y - Linkage via Consent");
            }
            if (rs.GetFieldValue(DataItemCodes.Consent.Research).Equals("Y"))
            {
                ConsentStrings.Add("Y - Research via Consent");
            }
            if (rs.GetFieldValue(DataItemCodes.Consent.Research).Equals("Y"))
            {
                ConsentStrings.Add("Y - Research via Consent");
            }
            if (rs.GetFieldValue(DataItemCodes.Consent.ContactFutureResearch).Equals("Y"))
            {
                ConsentStrings.Add("Y - Future Research via Consent");
            }
            if (rs.GetFieldValue(DataItemCodes.Consent.Companies).Equals("Y"))
            {
                ConsentStrings.Add("Y - Companies via Consent");
            }

            return string.Join(" ; ", ConsentStrings);

        }

        protected override string[] EnhancedHeaders()
        {
            var OriginalHeaders = OutputRecords.First().Record.OriginalFile.SpecificationFile.Fields.Select(x => x.Name).ToArray();

            var NewHeadersToAppend = new string[] {
                "IBDR_MonthofBirth",
                "IBDR_YearOfBirth",
                "IBDR_UKCountry",
                "IBDR_LSOA",
                "IBDR_MonthofDeath",
                "IBDR_YearofDeath",
                "IBDR_CreatedDateTime",
                "IBDR_CreatedByIBDAuditCode",
                "IBDR_UpdatedDateTime",
                "IBDR_UpdatedByIBDAuditCode",
                "IBDR_HASH",
                "IBDR_Source",
                "IBDR_Submission",
                "IBDR_DerivedAge",
                "IBDR_CurrentDiagnosis",
                "IBDR_DiseaseDuration",
                "IBDR_ReportGroup",
                "IBDR_SiteType",
                "IBDR_ConsentGroup",
                "IBDR_TableGeneratedDateTime"

            };

            return OriginalHeaders.AppendArray(NewHeadersToAppend);

        }

        protected override IEnumerable<Record> MergeRecords(IEnumerable<Record> records) => records.Distinct(new PatientRecordComparer());


        class PatientRecordComparer : IEqualityComparer<Record>
        {
            public bool Equals(Record x, Record y)
            {
                return x.GetValueByDataItemCode(DataItemCodes.NHSNumber).Equals(y.GetValueByDataItemCode(DataItemCodes.NHSNumber)) &&
                       x.GetValueByDataItemCode(DataItemCodes.DateOfBirth).Equals(y.GetValueByDataItemCode(DataItemCodes.DateOfBirth));
            }

            public int GetHashCode(Record obj)
            {
                return obj.GetValueByDataItemCode(DataItemCodes.NHSNumber).GetHashCode() ^ obj.GetValueByDataItemCode(DataItemCodes.DateOfBirth).GetHashCode();
            }
        }
    }
}