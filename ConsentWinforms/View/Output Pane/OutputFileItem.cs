using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.View
{
	public abstract class OutputFileItem : AbstractFileItem<OutputFile>
	{
		public OutputFileItem(OutputFile file) : base(file)
		{
		}

		
	}

}
