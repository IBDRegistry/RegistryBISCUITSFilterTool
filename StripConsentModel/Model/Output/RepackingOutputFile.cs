using StripConsentModel.Model.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static StripV3Consent.Model.ConsentToolModel;

namespace StripV3Consent.Model
{
	public abstract class RepackingOutputFile: OutputFile
	{
		public IEnumerable<RecordWithOriginalSet> OutputRecords;
		public IEnumerable<Record> AllRecordsOriginallyInFile;

		public RepackingOutputFile(DataFile file, IEnumerable<RecordWithOriginalSet> outputRecords, IEnumerable<Record> allRecordsOriginallyInFile) : base(file)
		{
			OutputRecords = outputRecords;
			AllRecordsOriginallyInFile = allRecordsOriginallyInFile;
		}

		private const string ColumnSeparator = ",";
		private const string RowSeparator = "\r\n";

		public static string RemoveConflictingChars(string StringToClean, string[] ExtraValuesToRemove = null)
		{
			string[] ValuesToRemove = { "\n", "\r", "," };

			ValuesToRemove = ValuesToRemove ?? ValuesToRemove.Union(ExtraValuesToRemove).ToArray();

			foreach (string value in ValuesToRemove)
			{
				StringToClean = StringToClean.Replace(value, "");
			}

			return StringToClean;
		}



		public static string RepackIntoString(IEnumerable<string[]> records, IEnumerable<string> HeaderFields)
		{
			List<string[]> Content = records.ToList<string[]>();

			string[] Headers = HeaderFields.ToArray();
			Headers[0] = "HEADER_" + Headers[0];    //First header must begin with HEADER_


			string[][] OutputFileRows = new string[Content.Count() + 1][];  //the 1 is for the header row
			
			OutputFileRows[0] = Headers.Select(header => RemoveConflictingChars(header)).ToArray();
			Content.CopyTo(OutputFileRows, 1);

			return string.Join(RowSeparator, OutputFileRows.Select(
								record => string.Join(ColumnSeparator, record)));
		}

		/// <summary>
		/// Some of the records coming up are the wrong length, this adds empty values to the end so that enhancement fields when added to the end go in the right place
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		private RecordWithOriginalSet NormaliseRecord(RecordWithOriginalSet record)
		{
			var DesiredLength = record.Record.OriginalFile.SpecificationFile.Fields.Count;
			var ActualLength = record.Record.DataRecord.Length;
			var EmptyValuesToAppend = new List<string>();
            for (int i = 0; i < DesiredLength - ActualLength; i++)
            {
				EmptyValuesToAppend.Add("");
            }
			var newRecord = record.Record.DataRecord.AppendArray(EmptyValuesToAppend.ToArray());

			return new RecordWithOriginalSet(
				new Record(newRecord, record.Record.OriginalFile), 
				record.OriginalSet
				);
		}

		protected abstract string[] EnhanceRecord(RecordWithOriginalSet record);

		protected abstract string[] EnhancedHeaders();

		private string EnhanceAndRepack()
        {
			var NormalisedRecords = OutputRecords.Select(NormaliseRecord);
			var EnhancedRecords = NormalisedRecords.Select(EnhanceRecord).ToList();

			return RepackIntoString(EnhancedRecords, EnhancedHeaders());
        }
		public override string StringOutput() => EnhanceAndRepack();

		public override string ToString() => StringOutput();
    }




}
