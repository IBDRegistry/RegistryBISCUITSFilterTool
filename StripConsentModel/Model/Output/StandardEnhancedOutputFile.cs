using StripV3Consent.Model;
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

		protected string IBDR_CreatedDateTime(Record record) => record.OriginalFile.FileCreatedTimestamp.ToString("dd-MM-yyyy HH:mm");

		protected string IBDR_CreatedByIBDAuditCode(Record record) => record.OriginalFile.Batch.Files
				.SelectMany(f => f.Records)
				.OrderBy(r => r.OriginalFile.FileCreatedTimestamp)
				.First()
				.OriginalFile.Batch.TrustInfo
				.IBDAuditCode;

		protected string IBDR_UpdatedDateTime(Record record) => record.OriginalFile.FileCreatedTimestamp.ToString("dd-MM-yyyy HH:mm");

		protected string IBDR_UpdatedByIBDAuditCode(Record record) => record.OriginalFile.Batch.TrustInfo.IBDAuditCode;

		protected string IBDR_Hash(Record record)
        {
			var NhsNumber = record.GetValueByDataItemCode(DataItemCodes.NHSNumber);
			return NhsNumber.CreateMD5();
        }

		protected string IBDR_Source => "";

		protected string IBDR_Submission(Record record) => record.OriginalFile.FileCreatedTimestamp.ToString("MM/yyyy");

		protected override string[] EnhanceRecord(RecordWithOriginalSet combinedRecord)
		{
			var record = combinedRecord.Record;

			string[] ToAppend = new string[] { 
				IBDR_CreatedDateTime(record), 
				IBDR_CreatedByIBDAuditCode(record), 
				IBDR_UpdatedDateTime(record), 
				IBDR_UpdatedByIBDAuditCode(record), 
				IBDR_Hash(record), 
				IBDR_Source, 
				IBDR_Submission(record) 
			};

			return record.DataRecord.AppendArray(ToAppend);
		}

        protected override string[] EnhancedHeaders()
        {
			var OriginalHeaders = OutputRecords.First().Record.OriginalFile.SpecificationFile.Fields.Select(x => x.Name).ToArray();

			var NewHeadersToAppend = new string[] {
				"IBDR_CreatedDateTime", "IBDR_CreatedByIBDAuditCode", "IBDR_UpdatedDateTime", "IBDR_UpdatedByIBDAuditCode", "IBDR_HASH", "IBDR_Source", "IBDR_Submission"
			};

			return OriginalHeaders.AppendArray(NewHeadersToAppend);

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
