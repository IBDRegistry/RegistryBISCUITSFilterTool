using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripV3Consent.Model
{
	public static class ConsentValidation
	{
		private static ConsentState HasIndividualRecordConsented(Record ConsentRecord)
		{
			//Is Consent V4 and Yes
			string ConsentVersion = ConsentRecord.GetValueByDataItemCode(DataItemCodes.Consent.Version);
			string RegistryConsent = ConsentRecord.GetValueByDataItemCode(DataItemCodes.Consent.Registry);

			if (ConsentVersion == "V4" & RegistryConsent == "Y") //Check for Date of Consent as well
			{
				return new V4Consent();
			}

			if (RegistryConsent != "Y")
			{
				return new NoConsent();
			}

			if (ConsentVersion.ToUpper() != "V4" & RegistryConsent == "Y")
			{
				return new OldConsent();
			}

			//Date of Consent
			//User cannot save page unless version of consent, registry consent and date of consent are provided


			return new UnknownInvalidConsent();

			//If Consent record is missing then true
			//If there is an entry and consent=Yes and consent is v4 then true
			//All other cases false
			//If consentv4 but no consent then false
			//If consentv3 but no consent then false
		}

		private const string ConsentFileNameInSpec = "consent";
		private static bool IsConsentRecord(Record r) => r.OriginalFile.SpecificationFile.Name.Contains(ConsentFileNameInSpec);

		public static ConsentState IsConsentValid(RecordSet recordset, bool EnableNationalOptOut)
		{
			var Records = recordset.Records;

			IEnumerable<Record> AllConsentRecords = Records.Where(IsConsentRecord);


			// S251
			if (AllConsentRecords.Count() == 0)
			{
				//National Opt-Out
				IEnumerable<Record> NationalOptOutRecords = Records.Where(r => r.OriginalFile.SpecificationFile is Specification.NationalOptOutFile);
				if (EnableNationalOptOut == true && NationalOptOutRecords.Count() == 0)
				{
					return new NationalOptOut();
				}
				else
				{
					return new S251();
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


			if (AllConsentRecords.Count() > 0 & ValidConsentRecords.Count() == 0)
			{
				return new ErroneousConsent();
			}


			return HasIndividualRecordConsented(ConsentRecord);

		}

		private static bool IsValidConsentRecord(Record r)
		{
			string RawValue = r.GetValueByDataItemCode(DataItemCodes.Consent.DateConsentLastRecorded);
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

		/// <summary>
		/// Compared Record by date and uses consent value as a tie breaker
		/// </summary>
		public class ConsentRecordComparator : IComparer<Record>
		{
			public int Compare(Record x, Record y)
			{
				if (IsConsentRecord(x) != true || IsConsentRecord(y) != true)
				{
					throw new Exception("ConsentRecordComparator provided with records that are not consent records");
				}

				if (IsValidConsentRecord(x) != true || IsValidConsentRecord(y) != true)
				{
					throw new Exception("ConsentRecordComparator provided with consent records with invalid dates");
				}

				DateTime x_DateOfConsent = DateTime.Parse(x.GetValueByDataItemCode(DataItemCodes.Consent.DateConsentLastRecorded));
				DateTime y_DateOfConsent = DateTime.Parse(y.GetValueByDataItemCode(DataItemCodes.Consent.DateConsentLastRecorded));

				int order = x_DateOfConsent.CompareTo(y_DateOfConsent);
				if (order == 0) //these records have exactly the same DateTime
				{
					ConsentState x_Consent = HasIndividualRecordConsented(x);
					ConsentState y_Consent = HasIndividualRecordConsented(y);

					//In the event of matching DateTimes the tiebreaker should choose the consent record with incorrect date
					if (x_Consent.Valid == false & y_Consent.Valid == true)
					{
						return 1;  //x is "greater than" y and will outrank it in OrderByDescending
					}
					else if (x_Consent.Valid == true & y_Consent.Valid == false)
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

	}
	public abstract class ConsentState {
		public abstract bool Valid { get; }
		public abstract string UIMessage { get; }

		//public abstract string PatientEnhancedMessage { get; }
	}

	public abstract class ValidConsent : ConsentState { 
		public override bool Valid => true;
	}

	public class S251 : ValidConsent {
        public override string UIMessage => "No consent record so patient flows via s251";
    }

	public class V4Consent : ValidConsent {
		public override string UIMessage => "V4 consent version and consented patient";
	}

	public abstract class InvalidConsent : ConsentState {
		public override bool Valid => false;
	}

	public class ErroneousConsent : InvalidConsent {
		public override string UIMessage => "Consent record(s) not valid";
	}

	public class NationalOptOut : InvalidConsent {
		public override string UIMessage => "Patient removed to ensure compliance with national data opt-out";
	}

	public class NoConsent : InvalidConsent {
		public override string UIMessage => "Registry consent was not \"Yes\"";
	}

	public class OldConsent : InvalidConsent {
		public override string UIMessage => "Consented patients are no longer valid unless they hold V4 consent";
	}

	public class UnknownInvalidConsent : InvalidConsent {
		public override string UIMessage => "Patient did not pass consent checks";
	}
}
