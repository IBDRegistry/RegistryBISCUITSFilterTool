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
        
        
        public static class Biologics
        {
            public static readonly string DateOfInitiationOrReviewEvent = "IBD201";
            public static readonly string DateOfInitialTreatmentFirstLoadingDose = "IBD211";
        }
        
        public static class Contact
        {
            public static readonly string DateOfContact = "IBD20";
            public static readonly string DateOfDiseaseCharacteristics = "IBD357";
        }
        public static class Medication
        {
            public static readonly string DrugStartDate = "IBD76";
            public static readonly string DrugEndDate = "IBD77";
            public static readonly string DrugChangeDate = "IBD366";
            public static readonly string BiologicsDateOfInitiationOrReviewEvent = "IBD201";
        }
        
        public static class Surgery
        {
            public static readonly string DateOfSurgery = "IBD43";
        }

        public static class Admission
        {
            public static readonly string DateOfAdmission = "IBD39";
            public static readonly string DateOfDischarge = "IBD42";
        }


        public static class Diagnosis {
            public static readonly string DateOfCurrentDiagnosis = "IBD08";
            public static readonly string DateOfEarliestDiagnosis = "IBD12";
        }

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

        public static readonly string[] DateFields = { 
            Biologics.DateOfInitiationOrReviewEvent,
            Biologics.DateOfInitialTreatmentFirstLoadingDose,
            Contact.DateOfContact,
            Contact.DateOfDiseaseCharacteristics,
            Medication.DrugStartDate,
            Medication.DrugEndDate,
            Medication.DrugChangeDate,
            Medication.BiologicsDateOfInitiationOrReviewEvent,
            Surgery.DateOfSurgery,
            Admission.DateOfAdmission,
            Admission.DateOfDischarge,
            Diagnosis.DateOfCurrentDiagnosis,
            Diagnosis.DateOfEarliestDiagnosis
        };
    }
}
