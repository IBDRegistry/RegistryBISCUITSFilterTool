using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public class LoadedFile: DataFile
    {
        public string[][] Content;

        private const string ColumnSeparator = ",";
        private const string RowSeparator = "\r\n";

        public LoadedFile(FileInfo file, string[][] content) : base(file)
        {
            File = file;
            Content = content;
        }

        public string RepackIntoString()
        {
            return String.Join(RowSeparator, Content.Select(
                                record => String.Join(ColumnSeparator, record)));
        }

        public static explicit operator string(LoadedFile file) => file.RepackIntoString();
        
        public override string ToString()
        {
            return File.Name;
        }
    }
}
