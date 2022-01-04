using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripV3Consent.Model
{
    /// <summary>
    /// A collection of all the records pertaining to a single patient
    /// </summary>
    public class RecordSet
    {
        public List<Record> Records;

        public RecordSet()
        {

        }

        /// <summary>
        /// Returns the value of a field that has the supplied data item code
        /// </summary>
        /// <param name="SpecifiedDataItemCode">Data item code of the desired field, can be found in the static DataItemCodes class</param>
        /// <returns></returns>
        public string GetFieldValue(string SpecifiedDataItemCode)
        {
            Record RecordWithValue = Records.Where(r => r.OriginalFile.SpecificationFile.Fields.Where(Field => Field.DataItemCode == SpecifiedDataItemCode).Count() > 0).First();
            return RecordWithValue[RecordWithValue.OriginalFile.SpecificationFile.Fields.FindIndex(f => f.DataItemCode == SpecifiedDataItemCode)];
        }

        public ConsentValidState IsConsentValid
        {
            get
            {
                const string ConsentFileNameInSpec = "consent";

                Record[] ConsentRecords = Records.Where(r => r.OriginalFile.SpecificationFile.SimplifiedName == ConsentFileNameInSpec).ToArray();

                if (ConsentRecords.Length > 1)
                {
#warning ask SG
                    throw new OnlyOneRecordExpectedException()
                    {
                        NHSNumber = GetFieldValue(DataItemCodes.NHSNumber),
                        MessageToAppend = $"There are {ConsentRecords.Length} records from the {String.Join(" & ", ConsentRecords.Select(r => r.OriginalFile.Name))} files when there is only supposed to be 1 "
                    };

                }

                // No consent record
                if (ConsentRecords.Length == 0)
                {
                    return new ConsentValidState()
                    {
                        IsValid = true,
                        IsValidReason = "No consent record so patient flows via S251"
                    };
                }

                Record ConsentRecord = ConsentRecords.First();

                //Is Consent V4 and Yes
                string ConsentVersion = ConsentRecord.GetValueByDataItemCode(DataItemCodes.ConsentVersionFieldID);
                string RegistryConsent = ConsentRecord.GetValueByDataItemCode(DataItemCodes.ConsentForRegistryFieldID);

                if (ConsentVersion == "V4" & RegistryConsent == "Y") //Check for Date of Consent as well
                {
                    return new ConsentValidState()
                    {
                        IsValid = true,
                        IsValidReason = "V4 consent version and consented patient"
                    };
                }

                if (RegistryConsent != "Y")
                {
                    return new ConsentValidState()
                    {
                        IsValid = false,
                        IsValidReason = "Registry consent was not \"Yes\""
                    };
                }

#warning add check for ConsentVersion == "Pre-V4" as well as blank
                if (ConsentVersion != "V4" & RegistryConsent == "Y")
                {
                    return new ConsentValidState()
                    {
                        IsValid = false,
                        IsValidReason = "Consented patients are no longer valid unless they are Consent V4+"
                    };
                }

                //Date of Consent
                //User cannot save page unless version of consent, registry consent and date of consent are provided


#warning use both NHS Number and DOB for grouping
                return new ConsentValidState()
                {
                    IsValid = false,
                    IsValidReason = "Patient did not pass consent checks"
                };

                //If Consent record is missing then true
                //If there is an entry and consent=Yes and consent is v4 then true
                //All other cases false
                //If consentv4 but no consent then false
                //If consentv3 but no consent then false

            }
        }

        public override string ToString()
        {
            return GetFieldValue(DataItemCodes.NHSNumber);
        }


    }

    public class ConsentValidState
    {
        public bool IsValid;

        /// <summary>
        /// Populated with the reason for passing/failing
        /// </summary>
        public string IsValidReason;


        public static implicit operator bool(ConsentValidState c) => c.IsValid;
    }
}
