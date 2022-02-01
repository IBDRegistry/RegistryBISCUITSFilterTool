using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public class OutputFile: DataFile
    {
        public IEnumerable<Record> OutputRecords;
        public IEnumerable<Record> AllRecordsOriginallyInFile;

        private const string ColumnSeparator = ",";
        private const string RowSeparator = "\r\n";

        public OutputFile(DataFile file) : base(file.Name) { }
        

        public string RepackIntoString()
        {
            string[][] Content = OutputRecords.Select(r => r.DataRecord).ToArray();

            return String.Join(RowSeparator, Content.Select(
                                record => String.Join(ColumnSeparator, record)));
        }

        public static explicit operator string(OutputFile file) => file.RepackIntoString();
        
        public override string ToString()
        {
            return Name;
        }
    }
}
