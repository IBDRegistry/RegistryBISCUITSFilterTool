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
			Record RecordWithValue = Records.Where(r => r.OriginalFile.SpecificationFile.Fields
																.Where(Field => Field.DataItemCode == SpecifiedDataItemCode)
																.Count() > 0
											 ).FirstOrDefault();

			if (RecordWithValue is null)
				return null;

			return RecordWithValue[RecordWithValue.OriginalFile.SpecificationFile.Fields.FindIndex(f => f.DataItemCode == SpecifiedDataItemCode)];
		}

		internal const string ConsentFileNameInSpec = "consent";
		internal static bool IsConsentRecord(Record r) => r.OriginalFile.SpecificationFile.Name.Contains(ConsentFileNameInSpec);

		internal static bool IsValidConsentRecord(Record r) {
			string RawValue = r.GetValueByDataItemCode(DataItemCodes.DateOfConsent);
			DateTime CastValue;
			if (DateTime.TryParse(RawValue, out CastValue))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		internal static ConsentValidState HasIndividualRecordConsented(Record ConsentRecord)
		{
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

			if (ConsentVersion.ToUpper() != "V4" & RegistryConsent == "Y")
			{
				return new ConsentValidState()
				{
					IsValid = false,
					IsValidReason = "Consented patients are no longer valid unless they hold V4 consent"
				};
			}

			//Date of Consent
			//User cannot save page unless version of consent, registry consent and date of consent are provided


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


		public ConsentValidState IsConsentValid
		{
			get
			{
				

				IEnumerable<Record> AllConsentRecords = Records.Where(IsConsentRecord);


				// S251
				if (AllConsentRecords.Count() == 0)
				{
					//National Opt-Out
					IEnumerable<Record> NationalOptOutRecords = Records.Where(r => r.OriginalFile.SpecificationFile is Specification.NationalOptOutFile);
					if (ConsentToolModel.EnableNationalOptOut == true && NationalOptOutRecords.Count() == 0)
					{
						return new ConsentValidState()
						{
							IsValid = false,
							IsValidReason = "Patient removed to ensure compliance with national data opt-out"
						};
					} else 
					{ 
						return new ConsentValidState()
						{
							IsValid = true,
							IsValidReason = "No consent record so patient flows via s251"
						};
					}
				}


				IEnumerable<Record> ValidConsentRecords = AllConsentRecords.Where(IsValidConsentRecord);

				Record ConsentRecord = null;

				if (ValidConsentRecords.Count() == 1)
				{
					ConsentRecord = ValidConsentRecords.First();
				}
				else if (ValidConsentRecords.Count() > 1)
				{
					ConsentRecord = ValidConsentRecords.OrderByDescending(r => r, new ConsentRecordComparator()).First();
				}


				if (AllConsentRecords.Count() > 0 & ValidConsentRecords.Count() == 0 )
				{
					return new ConsentValidState()
					{
						IsValid = false,
						IsValidReason = "Consent record(s) not valid"
					};
				}


				return HasIndividualRecordConsented(ConsentRecord);

			}
		}

		public override string ToString()
		{
			return GetFieldValue(DataItemCodes.NHSNumber);
		}


	}

    /// <summary>
    /// Compared Record by date and uses consent value as a tie breaker
    /// </summary>
    public class ConsentRecordComparator : IComparer<Record>
    {
        public int Compare(Record x, Record y)
        {
			if (RecordSet.IsConsentRecord(x) != true || RecordSet.IsConsentRecord(y) != true)
            {
				throw new Exception("ConsentRecordComparator provided with records that are not consent records");
            }

			if (RecordSet.IsValidConsentRecord(x) != true || RecordSet.IsValidConsentRecord(y) != true)
			{
				throw new Exception("ConsentRecordComparator provided with consent records with invalid dates");
			}

			DateTime x_DateOfConsent = DateTime.Parse(x.GetValueByDataItemCode(DataItemCodes.DateOfConsent));
			DateTime y_DateOfConsent = DateTime.Parse(y.GetValueByDataItemCode(DataItemCodes.DateOfConsent));

			int order = x_DateOfConsent.CompareTo(y_DateOfConsent);
			if (order == 0) //these records have exactly the same DateTime
			{
				ConsentValidState x_Consent = RecordSet.HasIndividualRecordConsented(x);
				ConsentValidState y_Consent = RecordSet.HasIndividualRecordConsented(y);

				//In the event of matching DateTimes the tiebreaker should choose the consent record with incorrect date
				if (x_Consent.IsValid == false & y_Consent.IsValid == true)
				{
					return 1;  //x is "greater than" y and will outrank it in OrderByDescending
				}
				else if (x_Consent.IsValid == true & y_Consent.IsValid == false)
				{
					return -1;   //y is "greater than" x
				}
				else // false false and true true
				{
					return 0;  //it doesn't really matter, they're both good
				}

			}
			else //these records have different date times so return the order so that OrderByDescending can pick the newer one
            {
				return order;
            }
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
