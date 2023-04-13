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

		public RecordSet(bool ShouldNationalOptOutBeChecked)
		{
			EnableNationalOptOut = ShouldNationalOptOutBeChecked;
		}

		public IEnumerable<Record> GetRecordsWithDataItemCode(string SpecifiedDataItemCode)
        {
			IEnumerable<Record> RecordsWithValue = Records.Where(r => r.OriginalFile.SpecificationFile.Fields
																.Where(Field => Field.DataItemCode == SpecifiedDataItemCode)
																.Count() > 0
																);

			Func<Record, int> GetIndexOfInRecord = (Record r) => r.OriginalFile.SpecificationFile.Fields.FindIndex(f => f.DataItemCode == SpecifiedDataItemCode);

			Func<Record, bool> isRecordValid = (Record r) => GetIndexOfInRecord(r) < r.DataRecord.Length - 1;

			IEnumerable<Record> ValidRecordsWithValue = RecordsWithValue.Where(isRecordValid);

			return ValidRecordsWithValue;
		}

		/// <summary>
		/// Returns the value of a field that has the supplied data item code
		/// </summary>
		/// <param name="SpecifiedDataItemCode">Data item code of the desired field, can be found in the static DataItemCodes class</param>
		/// <returns></returns>
		public string[] GetFieldValue(string SpecifiedDataItemCode)
		{
			IEnumerable<Record> ValidRecordsWithValue = GetRecordsWithDataItemCode(SpecifiedDataItemCode);

			if (ValidRecordsWithValue.Count() == 0)
				return new string[] {""};//return array containing empty string rather than Array.Empty for backwards compatibility as GetFieldValue was never expected to return null

			return ValidRecordsWithValue.Select(r => r.GetValueByDataItemCode(SpecifiedDataItemCode)).ToArray();
		}

		private bool EnableNationalOptOut;
		 
		public ConsentViewState IsConsentValid
		{
			get
			{
				var ValidState = ConsentValidation.IsConsentValid(this, EnableNationalOptOut);

				return new ConsentViewState()
				{
					IsValid = ValidState.Valid,
					IsValidReason = ValidState.UIMessage
				};
			}
		}

		public override string ToString()
		{
			return GetFieldValue(DataItemCodes.NHSNumber).First();
		}


	}


    public class ConsentViewState
	{
		public bool IsValid;

		/// <summary>
		/// Populated with the reason for passing/failing
		/// </summary>
		public string IsValidReason;


		public static implicit operator bool(ConsentViewState c) => c.IsValid;
	}

	
}
