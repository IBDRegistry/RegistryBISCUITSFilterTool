﻿using StripConsentModel.Model.Common;
using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static StripV3Consent.Model.ConsentToolModel;

namespace StripConsentModel.Model.Output
{
    class StandardEnhancedOutputFile : RepackingOutputFile
    {
        public StandardEnhancedOutputFile(DataFile file, IEnumerable<RecordWithOriginalSet> outputRecords, IEnumerable<Record> allRecordsOriginallyInFile) : base(file, outputRecords, allRecordsOriginallyInFile)
        {
        }

		protected string IBDR_CreatedDateTime(Record record) => record.OriginalFile.FileModifiedTimestamp.ToString("dd-MM-yyyy HH:mm");

		protected string IBDR_CreatedByIBDAuditCode(RecordSet rs)
		{
			return rs.Records
				.OrderBy(r => r.OriginalFile.FileModifiedTimestamp)
				.Where(r => r.SiteLookup != null)
				.FirstOrDefault()
				.SiteLookup.IBDAuditCode;
		}
		//private string LocalUnitCodeOverAuditCode(Record r)
  //      {
		//	if (r == null)
		//	{
		//		return null;
		//	}

		//	var LocalUnitCode = r.GetValueByDataItemCode(DataItemCodes.LocalUnitCode);
		//	if (!string.IsNullOrEmpty(LocalUnitCode) && LocalUnitCode != "IBD000" && LocalUnitCode != "NA")
		//	{
		//		if (!LocalUnitCode.StartsWith("IBD"))
  //              {
		//			throw new System.Exception();
  //              }
		//		return LocalUnitCode;
		//	}

		//	var IBDAuditCode = r.GetValueByDataItemCode(DataItemCodes.IBDAuditCode);
		//		return IBDAuditCode;
		//}


		protected string IBDR_UpdatedByIBDAuditCode(RecordSet rs) =>  rs.Records
				.OrderByDescending(r => r.OriginalFile.FileModifiedTimestamp)
				.Where(r => r.SiteLookup != null)
				.FirstOrDefault()
				.SiteLookup.IBDAuditCode;
														

		protected string IBDR_UpdatedDateTime(Record record) => record.OriginalFile.FileModifiedTimestamp.ToString("dd-MM-yyyy HH:mm");

		protected string IBDR_Hash(Record record)
        {
			var NhsNumber = record.GetValueByDataItemCode(DataItemCodes.NHSNumber);
			return NhsNumber.CreateMD5();
        }

		protected string IBDR_Source => "";

		protected string IBDR_Submission(Record record) => record.OriginalFile.FileModifiedTimestamp.ToString("MM/yyyy");

		private Record ProcessDateFields(Record record)
		{
			var specificationFields = record.OriginalFile.SpecificationFile.Fields;

			if (specificationFields.Select(x => x.DataItemCode).Intersect(DataItemCodes.DateFields).Count() == 0)
				return record;

			for (int i=0; i < record.DataRecord.Length; i++)
            {
				if (DataItemCodes.DateFields.Contains(specificationFields[i].DataItemCode))
                {
					string possibleDateValue = record[i];

					DateTime parsed;
					bool didParseCorrectly = DateTime.TryParse(possibleDateValue, out parsed);

					if (didParseCorrectly)
						record[i] = parsed.ToString("yyyy-MM-dd");
                }
            }

			return record;
		}

		protected override Record EnhanceRecord(RecordWithOriginalSet combinedRecord)
		{
			var record = combinedRecord.Record;
			var ProcessedRecord = ProcessDateFields(record);

			string[] ToAppend = new string[] { 
				IBDR_CreatedDateTime(record), 
				IBDR_CreatedByIBDAuditCode(combinedRecord.OriginalSet), 
				IBDR_UpdatedDateTime(record), 
				IBDR_UpdatedByIBDAuditCode(combinedRecord.OriginalSet), 
				IBDR_Hash(record), 
				IBDR_Source, 
				IBDR_Submission(record) 
			};

			return new Record(record.DataRecord.AppendArray(ToAppend), combinedRecord.Record.OriginalFile);
		}



        protected override string[] EnhancedHeaders()
        {

			var NewHeadersToAppend = new string[] {
				"IBDR_CreatedDateTime", "IBDR_CreatedByIBDAuditCode", "IBDR_UpdatedDateTime", "IBDR_UpdatedByIBDAuditCode", "IBDR_HASH", "IBDR_Source", "IBDR_Submission"
			};

			return OriginalHeaders().AppendArray(NewHeadersToAppend);

        }
	}

	public static class ArrayExtensions
    {
		public static string[] AppendArray(this string[] record, string[] ToAppend)
        {
			string[] JoinedArrays = new string[record.Length + ToAppend.Length];
			record.CopyTo(JoinedArrays, 0);
			ToAppend.CopyTo(JoinedArrays, record.Length);

			return JoinedArrays;
		}

		public static void EditValueByDataItemCode(this Record record, string dataItemCode, string value)
        {
			int FieldIndex = record.OriginalFile.SpecificationFile.Fields.FindIndex(f => f.DataItemCode == dataItemCode);
			record[FieldIndex] = value;
		}
    }

	public static class StringCryptographyExtensions
    {
		public static string CreateMD5(this string input)
		{
			// Use input string to calculate MD5 hash
			using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
			{
				byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
				byte[] hashBytes = md5.ComputeHash(inputBytes);


                 StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
		}
	}
}
