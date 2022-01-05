using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public class NationalOptOutFile : DataFile
    {

        public NationalOptOutFile(string path) : base(path)
        {
        }

        public FileValidationState IsValid
        {
            get
            {
                FileValidationState ReturnValue = new FileValidationState();

                if (File.Extension != ".dat") { return new FileValidationState() { IsValid = ValidState.Error, Message = "File not DAT type" }; };


                if (IsCommaDelimited() != true)
                {
                    return new FileValidationState() { IsValid = ValidState.Error, Message = "DAT file not comma separated" };
                }


                return new FileValidationState() { IsValid = ValidState.Good, Message = "File passed validation checks" };
            }
        }

        private bool IsCommaDelimited()
        {
            //Crudely tries to find if the file is comma delimited by seeing what is the most common char out of common delimiters
            //Especially tab as excel loves to swap commas for tabs in csv files
            char[] CommonDelimiters = new char[] {',','\t', '|' };

            string FileContents = null;
            using (StreamReader StreamReader = new StreamReader(this.Path))
            {
                FileContents = StreamReader.ReadToEnd();
            }
            int[] AppearanceCounts = CommonDelimiters.Select(Delimiter => FileContents.Count(f => f == Delimiter)).ToArray();

            if (AppearanceCounts[Array.IndexOf(CommonDelimiters, ',')] == AppearanceCounts.Max())
            {
                return true;
            } else
            {
                return false;
            }
        }

        private File2DArray SplitIntoBoxed2DArrayWithHeaders(string File)
        {
            string RowSeparator = "\r\n";       
            char ColumnSeparator = ',';

            string[][] TwoDList;
            string[] ListOfLines = File.Split(RowSeparator.ToCharArray());

            TwoDList = ListOfLines.Select(x => x.Split(ColumnSeparator)).ToArray();

            File2DArray Return2DArray = new File2DArray();

            string[][] EmptyRows = (from string[] element in TwoDList
                                    where element.IsEmpty()
                                    select element).ToArray();

            List<string[]> RowstoRemove = new List<string[]>();
            RowstoRemove.AddRange(EmptyRows);

            Return2DArray.Content = TwoDList.Except(RowstoRemove).ToArray();


            return Return2DArray;
        }

        public string[][] SplitInto2DArray()
        {
            string FileContent = null;
            using (StreamReader reader = new StreamReader(this.Path))
            {
                FileContent = reader.ReadToEnd();
            }

            return SplitIntoBoxed2DArrayWithHeaders(FileContent);
        }
    }
}
