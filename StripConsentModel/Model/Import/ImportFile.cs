using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public class ImportFile : DataFile
    {
        public ImportFile(string path) : base(path)
        {
        }

        public FileValidationState IsValid
        {
            get
            {
                FileValidationState ReturnValue = new FileValidationState();

                if (File.Extension == ".csv")
                {
                    string[] SpecificationFileNames = Spec2021K.Specification.PatientFiles.Select(SpecificationFile => SpecificationFile.SimplifiedName).ToArray();

                    //If any of the words from 2021K's filenames (patient, consent, contact, admission) are in the current filename
                    if (SpecificationFileNames.Select(SpecificationFileName => File.Name.Contains(SpecificationFileName)).Contains(true))
                    {
                        ReturnValue.Organisation = FileOrganisation.Registry;

                        if (IsCommaDelimited() == true)
                        {
                            ReturnValue.IsValid = ValidState.Good;
                            ReturnValue.Message = "File passed validation checks";
                        } else
                        {
                            ReturnValue.IsValid = ValidState.Error;
                            ReturnValue.Message = "CSV file not comma separated";
                            
                        }
                    }
                    else
                    {
                        ReturnValue.Organisation = FileOrganisation.Unknown;
                        ReturnValue.IsValid = ValidState.Error;
                        ReturnValue.Message = "CSV File not found in 2021K standard";
                    }
                } else if(File.Extension == ".dat")
                {
                    ReturnValue.Organisation = FileOrganisation.NHS;
                    ReturnValue.IsValid = ValidState.Warning;
                    ReturnValue.Message = "National Opt-Out file";
                } else
                {
                    ReturnValue.Organisation = FileOrganisation.Unknown;
                    ReturnValue.IsValid = ValidState.None;
                    ReturnValue.Message = "Unknown file type";
                }

                return ReturnValue;
            }
        }

        private bool ContainsHeaders()
        {
            String TopLeftValue = null;
            const uint CutOffValue = 32;
            using (StreamReader StreamReader = new StreamReader(this.Path))
            {
                StringBuilder TopLeftValueBuilder = new StringBuilder();
                while ((char)StreamReader.Peek() != ',')
                {
                    TopLeftValueBuilder.Append((char)StreamReader.Read());
                        
                    if (TopLeftValueBuilder.Length >= CutOffValue)
                    {
                        break;
                    }
                }
                TopLeftValue = TopLeftValueBuilder.ToString();
            }

            if (TopLeftValue.StartsWith("HEADER_"))
            {
                return true;
            }
            else
            {
                return false;
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

        private string LineEndingsInFile()
        {

            using (StreamReader StreamReader = new StreamReader(this.Path))
            {
                while (StreamReader.EndOfStream == false)
                {
                    if ((char)StreamReader.Read() == '\r') //If carraige return is encountered followed by line feed then Windows
                    {
                        if ((char)StreamReader.Read() == '\n')
                        {
                            return "\r\n";
                        }
                    }
                    else if ((char)StreamReader.Read() == '\n')   //If line feed is encountered first then Unix
                    {
                        return "\n";
                    }
                }

                //Neither line ending found, doesn't really matter what we return but we'll return line feed
                return "\n";
            }

        }

        private File2DArray SplitIntoBoxed2DArrayWithHeaders(string File)
        {
            string RowSeparator = LineEndingsInFile();       
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

            if (ContainsHeaders())
            {
                RowstoRemove.Add(TwoDList[0]);  //Remove headers from content
                Return2DArray.Headers = TwoDList[0];
            }
            Return2DArray.Content = TwoDList.Except(RowstoRemove).ToArray();


            return Return2DArray;
        }

        public File2DArray SplitInto2DArray()
        {
            string FileContent = null;
            using (StreamReader reader = new StreamReader(this.Path))
            {
                FileContent = reader.ReadToEnd();
            }

            return SplitIntoBoxed2DArrayWithHeaders(FileContent);
        }
    }

    public static class StringArrayExtension
    {
        public static bool IsEmpty(this string[] Array)
        {
            if (Array.Length == 1 & Array[0] == "") { return true; }

            if (Array.All(element => element == Array[0])) { return true; }

            return false;
        }
    }

    public static class FileChecking
    {
        public enum RegistryFileCheckResult
        {
            IsRegistry,
            IsNotRegistry,
            IsNotCSV
        }
        public static RegistryFileCheckResult IsRegistryFile(string path)
        {
            if (Path.GetExtension(path) == ".csv")
            {
                string[] SpecificationFileNames = Spec2021K.Specification.PatientFiles.Select(SpecificationFile => SpecificationFile.SimplifiedName).ToArray();

                //If any of the words from 2021K's filenames (patient, consent, contact, admission) are in the current filename
                if (SpecificationFileNames.Select(SpecificationFileName => Path.GetFileNameWithoutExtension(path).Contains(SpecificationFileName)).Contains(true))
                {
                    return RegistryFileCheckResult.IsRegistry;
                }
                else
                {
                    return RegistryFileCheckResult.IsNotRegistry;
                }
            } else
            {
                return RegistryFileCheckResult.IsNotCSV;
            }
        }


    }

    
}
