using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public abstract class OutputFile: DataFile
    {
        public override string OutputName
		{
            get
			{
                return SpecificationFile.Name.Replace(@".*\", "").Replace(".csv", "_Processed.csv");
            }
		}

       

        public OutputFile(DataFile file) : base(file.Name) { }

        public abstract string StringOutput();

        public static explicit operator string(OutputFile file) => file.StringOutput();



        public override string ToString()
        {
            return Name;
        }
    }
}
