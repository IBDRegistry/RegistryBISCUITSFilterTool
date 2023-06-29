using StripConsentModel.Model.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public class ImportFile : DataFile
    {
        /// <summary>
        /// This is optional and helps with the file save dialog at the end by letting it choose the most common path of the input files
        /// This is no longer optional as it is now used for batch calculation
        /// </summary>
        public string FilePath { get; private set; }

        public string FileContents { get; private set; }

        private Record[] _cachedRecords;
        public Record[] Records
        {
            get
            {
                if (_cachedRecords == null)
                {
                    _cachedRecords = SplitInto2DArray().Content.Select(Row => new Record(Row, this)).ToArray();
                }

                return _cachedRecords;
            }
        }

        public DateTime FileCreatedTimestamp { get; private set; }

        public ImportFile(string FileName, string Contents, DateTime FileCreatedTimestamp, string FilePath) : base(FileName)
        {
            this.FileContents = Contents;
            this.FileCreatedTimestamp = FileCreatedTimestamp;
            this.FilePath = FilePath;
        }

        

        public FileValidationState IsValid
        {
            get
            {
                FileValidationState ReturnValue = new FileValidationState();

                if (Name.EndsWith(".csv") && !IsCommaDelimited())
                {
                    ReturnValue.IncreaseErrorLevelIfStronger(ValidState.Error);
                    ReturnValue.Messages.Add("CSV file not comma separated");
                }

                if (SpecificationFile is Specification.RegistryFile)
                {
                    ReturnValue.Organisation = FileOrganisation.Registry;
                    ReturnValue.Messages.Add("Registry Upload File");
                    ReturnValue.IncreaseErrorLevelIfStronger(ValidState.Good);

                } else if (SpecificationFile is Specification.NationalOptOutFile)
                {
                    ReturnValue.Organisation = FileOrganisation.NHS;
                    ReturnValue.Messages.Add("National Opt-Out file");
                    var RowsAndColumns = SplitIntoBoxed2DArrayWithHeaders(FileContents);

                    Func<string[], bool> RowOnlyHasOneColumn = row => row.Length == 1 || (row.Length == 2 && string.IsNullOrEmpty(row[1]));
                    Func<string[], bool> RowHasMultipleColumns = row => !RowOnlyHasOneColumn(row);
                    if (RowsAndColumns.Content.All(RowOnlyHasOneColumn))
					{
                        //ReturnValue.IncreaseErrorLevelIfStronger(ValidState.Good);
                        ReturnValue.IncreaseErrorLevelIfStronger(ValidState.Error);//don't accept NOO file for BISCUITS processing as it breaks some stuff


                    } else if(RowsAndColumns.Content.Any(RowHasMultipleColumns))
					{
                        ReturnValue.IncreaseErrorLevelIfStronger(ValidState.Warning);
                        ReturnValue.Messages.Add("Opt-Out file has incorrect number of columns - this could cause issues. Try to ensure your opt-out file only has 1 column containing the NHS Numbers of patients that have not opted-out ");
                    }
                } else
                {
                    Func<string, bool> CouldFileBePoorlyNamedNOOFile = FileName =>
                    {
                        Regex PossibleMatchesRegex = new Regex(".*(opt|s251).*");
                        return PossibleMatchesRegex.Match(FileName).Success;
                    };

                    if (CouldFileBePoorlyNamedNOOFile(Name)) {
                        ReturnValue.Organisation = FileOrganisation.NHS;
                        ReturnValue.IncreaseErrorLevelIfStronger(ValidState.Error);
                        ReturnValue.Messages.Add("You have added a file that looks like it might be a national data opt-out file but is incorrectly formatted. Please review the guidance to fix this.");
                    } else
					{
                        ReturnValue.Organisation = FileOrganisation.Unknown;
                        ReturnValue.IncreaseErrorLevelIfStronger(ValidState.Error);
                        ReturnValue.Messages.Add("Unknown file type");
                    }
                    
                }


                
                
                
                return ReturnValue;
            }
        }

        private enum IsWellFormed
        {
            NotWellFormed,
            JaggedRows,
        }

        /// <summary>
        /// Returns whether the file has the correct number of columns and that each row contains all of those columns
        /// </summary>
        /// <param name="SuspectedFile">Specification.File object representing the file you think it is to get the correct column form for</param>
        /// <returns></returns>
        private bool IsFileWellFormed(Specification.File SuspectedFile)
        {
            File2DArray Contents = SplitInto2DArray();

            if (Contents.Content.GroupBy<string[], int>(Row => Row.Count()).Count() > 1)
            {
                var test = Contents.Content.GroupBy<string[], int>(Row => Row.Count()).Count();
                return false;
            }
            return true;
        }

        private bool ContainsHeaders()
        {
			string TopLeftValue = null;
            const uint CutOffValue = 32;
            StringBuilder TopLeftValueBuilder = new StringBuilder();
            for (int i = 0; i < FileContents.Length; i++)
            {
                char CurrentChar = FileContents[i];
                if (CurrentChar == ',')
                {
                    break;
                }
                else
                {
                    TopLeftValueBuilder.Append(CurrentChar);
                }
                if (TopLeftValueBuilder.Length >= CutOffValue)
                {
                    break;
                }
            }
            TopLeftValue = TopLeftValueBuilder.ToString();
            if (TopLeftValue.ToUpper().StartsWith("HEADER_"))
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

            int[] AppearanceCounts = CommonDelimiters.Select(Delimiter => FileContents.Count(f => f == Delimiter)).ToArray();

            if (AppearanceCounts[Array.IndexOf(CommonDelimiters, ',')] == AppearanceCounts.Max())
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        /// Set by parent class, returns whether the specification file has already been dropped into the import pane
        /// </summary>
        public Predicate<ImportFile> IsSpecificationFileAlreadyImported;

        private string LineEndingsInFile()
        {

            for (int i = 0; i < FileContents.Length; i++) { 
                char CurrentChar = FileContents[i];
                if (CurrentChar == '\r') //If carraige return is encountered followed by line feed then Windows
                {
                    if (FileContents[i+1] == '\n')
                    {
                        return "\r\n";
                    }
                }
                else if (CurrentChar == '\n')   //If line feed is encountered first then Unix
                {
                    return "\n";
                }
            }

            //Neither line ending found, doesn't really matter what we return but we'll return line feed and deal with \r pollution
            return "\n";

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

        private File2DArray SplitInto2DArray()
        {
            return SplitIntoBoxed2DArrayWithHeaders(FileContents);
        }
    }

    
}
