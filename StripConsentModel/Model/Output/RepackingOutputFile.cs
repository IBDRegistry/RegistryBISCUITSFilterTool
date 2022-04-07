using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripV3Consent.Model
{
	public class RepackingOutputFile: OutputFile
	{
		public IEnumerable<Record> OutputRecords;
		public IEnumerable<Record> AllRecordsOriginallyInFile;

		public RepackingOutputFile(DataFile file, IEnumerable<Record> outputRecords, IEnumerable<Record> allRecordsOriginallyInFile) : base(file)
		{
			OutputRecords = outputRecords;
			AllRecordsOriginallyInFile = allRecordsOriginallyInFile;
		}

		private const string ColumnSeparator = ",";
		private const string RowSeparator = "\r\n";

		private string RemoveConflictingChars(string StringToClean)
		{
			string[] ValuesToRemove = { "\n", "\r", "," };

			foreach (string value in ValuesToRemove)
			{
				StringToClean = StringToClean.Replace(value, "");
			}

			return StringToClean;
		}

		public string RepackIntoString()
		{
			List<string[]> Content = OutputRecords.Select(r => r.DataRecord).ToList<string[]>();

			string[] Headers = OutputRecords.First().OriginalFile.SpecificationFile.Fields.Select(field => field.Name).ToArray();
			Headers[0] = "HEADER_" + Headers[0];    //First header must begin with HEADER_


			string[][] OutputFileRows = new string[Content.Count() + 1][];  //the 1 is for the header row

			OutputFileRows[0] = Headers.Select(header => RemoveConflictingChars(header)).ToArray();
			Content.CopyTo(OutputFileRows, 1);

			return string.Join(RowSeparator, OutputFileRows.Select(
								record => string.Join(ColumnSeparator, record)));
		}
		public override string StringOutput() => RepackIntoString();

		
	}


}
