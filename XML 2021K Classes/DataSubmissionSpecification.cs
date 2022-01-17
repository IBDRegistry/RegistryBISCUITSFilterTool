using System.Collections.Generic;
using System.Linq;
using System.IO;
using ExcelDataReader;

namespace Specification
{
    public class DataSubmissionSpecification
    {
        /// <summary>
        /// The data item codes of the identifiers, hardcoded as they aren't declared as identifiers in 2021K
        /// </summary>
        public static string[] IdentifierCodes = new string[] { "IBD01" };

        /// <summary>
        /// Files where each record is associated a Patient
        /// </summary>
        public List<File> PatientFiles = new List<File>();

        /// <summary>
        /// For provenance.csv and other files where each record is not associated with a patient
        /// </summary>
        public List<File> SubmissionFiles = new List<File>();

        public File[] AllFiles => PatientFiles.Union<File>(SubmissionFiles).ToArray();

        public DataSubmissionSpecification() {
        }

        public DataSubmissionSpecification(FileInfo SpecificationFile)
        {
            LoadDataFromFile(SpecificationFile);
            WipeOutEmptyFields();
            AddSpecialFiles();
        }

        public int PositionOfInEveryFile(string SpecifiedIdentifierCode)
        {
            IEnumerable<int> PositionsOfRepeatingField = PatientFiles.Select<File, int>(PatientFile => PatientFile.Fields.FindIndex(field => field.DataItemCode == SpecifiedIdentifierCode));

            if (PositionsOfRepeatingField.Any(x => x != PositionsOfRepeatingField.First())) //Are the positions the same in every file
            {   //If not
                throw new System.Exception("Identifier fields are not in the same position in every file");
            }

            return PositionsOfRepeatingField.First();
        }

        /// <summary>
        /// Finds the position of identifier columns. Identifier columns have the same data item code and are always repeated across files
        /// </summary>
        /// <returns></returns>
        public int[] IdentiferLocations()
        {
            IEnumerable<Field> AllFields = PatientFiles.SelectMany(File => File.Fields);

            Field[] DistinctFields = AllFields.Distinct(new FieldEqualityComparer()).ToArray();

            Field[] IdentifierFields = DistinctFields.Where(f => IdentifierCodes.Contains(f.DataItemCode)).ToArray();

            int[][] PositionsOfRepeatingFields = IdentifierFields.Select<Field, int[]>(IdentifierField => PatientFiles.Select<File, int>(File => File.Fields.FindIndex(field => field.DataItemCode == IdentifierField.DataItemCode)).ToArray()).ToArray();

            if (PositionsOfRepeatingFields.Any(x => x.Any(y => y != x[0]))) //Are the positions the same in every file
            {   //If not
                throw new System.Exception($"Identifier fields are not in the same position in every file");
            }

            return PositionsOfRepeatingFields.Select(x => x[0]).ToArray();
        }

        internal string CustomTrustFilenameSuffixRegEx = @".*";

        private void LoadDataFromFile(FileInfo SpecificationFile)
        {
            using (var stream = System.IO.File.Open(SpecificationFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        if (!reader.Name.Contains(".csv"))  //Skip sheets that are not patient_abc.csv or admission_abc.csv
                        {
                            continue;
                        }


                        RegistryFile NewOutputFile = new RegistryFile()
                        {
                            //The name replaces _abc with a RegEx that specifies any number of characters could be there
                            //Before this we need to replace . with an escaped . ("\.") as it is a special character in RegEx but we want it to retain it's original meaning of .
                            Name = reader.Name.Replace(".", @"\.").Replace("_abc", CustomTrustFilenameSuffixRegEx),  
                            IsRegistryFile = true
                        };

                        reader.Read();

                        int NumberOfColumns = reader.FieldCount;
                        NewOutputFile.Fields = new Field[NumberOfColumns].ToList();
                        for (int x = 0; x < NumberOfColumns; x++)
                        {
                            NewOutputFile.Fields[x] = new Field();
                        }

                        for (int x=0; x<NumberOfColumns; x++)
                        {
                            NewOutputFile.Fields[x].Name = reader.GetString(x);
                        }

                        reader.Read();
                        for (int x = 0; x < NumberOfColumns; x++)
                        {
                            NewOutputFile.Fields[x].Group = reader.GetString(x);
                        }

                        reader.Read();
                        for (int x = 0; x < NumberOfColumns; x++)
                        {
                            NewOutputFile.Fields[x].Version = reader.GetString(x);
                        }

                        reader.Read();
                        for (int x = 0; x < NumberOfColumns; x++)
                        {
                            NewOutputFile.Fields[x].DataItemCode = reader.GetString(x);
                        }

                        if (NewOutputFile.IsPatientLevelFile)
                        {
                            PatientFiles.Add(NewOutputFile);
                        }
                        else
                        {
                            SubmissionFiles.Add(NewOutputFile);
                        }

                    } while (reader.NextResult());

                }
        }
    }

        /// <summary>
        /// There is a bug in ExcelDataReader where reader.fieldcount() will report the sheets being wider than they really are resulting in empty fields sneaking into the object model. this deletes them.
        /// </summary>
        /// <param name="Files"></param>
        private void WipeOutEmptyFields()
        {
            for (int x=0; x< PatientFiles.Count; x++)
            {
                List<Field> FieldListToIterateOver = new List<Field>(PatientFiles[x].Fields);
                for (int y=0; y< FieldListToIterateOver.Count; y++)
                {
                    Field CurrentField = FieldListToIterateOver[y];
                    if (CurrentField.Name is null || CurrentField.DataItemCode is null)
                    {
                        PatientFiles[x].Fields.Remove(CurrentField);
                    }
                }
            }

        }

        /// <summary>
        /// Add special files like the national opt out to the specification
        /// </summary>
        private void AddSpecialFiles()
        {
            NationalOptOutFile NationalOptOutFile = new NationalOptOutFile()
            {
                Name = @".+\.dat",
                IsRegistryFile = false,
                Fields = new List<Field>()
                {
                    new Field()
                    {
                        Name = "NHS Number",
                        DataItemCode = "IBD01"  //Has to use the same data item code as the one from 2021K for linkage purposes
                    },
                    new Field()
                    {
                        Name = "Is Opted-Out",
                        DataItemCode = "NHS01"    //Allocated specially for National Opt Out as it doesn't have a IBD data item code
                    }
                }
            };

            PatientFiles.Add(NationalOptOutFile);

        }
    }
}
