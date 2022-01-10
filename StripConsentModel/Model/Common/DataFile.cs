using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace StripV3Consent.Model
{
    public abstract class DataFile
    {
        public FileInfo File;

        public string Path => File.FullName;

        public string Name => File.Name;

        public DataFile(string path)
        {
            File = new FileInfo(path);
        }
        public DataFile(FileInfo fileInfo)
        {
            File = fileInfo;
        }

        public Specification.File SpecificationFile { get
            {
                IEnumerable<Specification.File> FilesWithMatchingPattern = Spec2021K.Specification.AllFiles.Where<Specification.File>(f => new Regex(f.Name).IsMatch(Name));

                if (FilesWithMatchingPattern.Count() > 1)
                {
                    StringBuilder ErrorMessage = new StringBuilder($"Multiple specification files matched import file {Name}. Those files are: ");
                    foreach (Specification.File MatchingFile in FilesWithMatchingPattern) { ErrorMessage.Append($"{MatchingFile.Name} "); }
                    throw new Exception(ErrorMessage.ToString());
                }

                if (FilesWithMatchingPattern.Count() == 0)
                {
                    return null;
                }

                return FilesWithMatchingPattern.First();
            }
        }

        


        public override string ToString() => this.Name;
    }

    public class File2DArray
    {
        public string[] Headers;
        public string[][] Content;
        public static implicit operator string[][](File2DArray Box) => Box.Content;
    }

}
