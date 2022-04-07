using System;
using System.Collections.Generic;
using System.Text;

namespace StripV3Consent.Model
{
	public class DirectOutputFile : OutputFile
	{
		public string ContentToOutput;
		public DirectOutputFile(DataFile file, string contentToOutput) : base(file)
		{
			ContentToOutput = contentToOutput;
		}

		public override string StringOutput() => ContentToOutput;
	}
}
