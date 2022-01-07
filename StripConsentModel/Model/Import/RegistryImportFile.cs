using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripV3Consent.Model
{
    public class RegistryImportFile: ImportFile
    {
        public RegistryImportFile(string path) :base(path)
        {

        }

        //public override FileValidationState IsValid
        //{
        //    get
        //    {
        //        FileValidationState ReturnValue = new FileValidationState();

        //        if (File.Extension != ".csv") { return new FileValidationState() { IsValid = ValidState.Error, Message = "File not CSV type" }; };

        //        string[] SpecificationFileNames = Spec2021K.Specification.PatientFiles.Select(SpecificationFile => SpecificationFile.SimplifiedName).ToArray();

        //        //If any of the words from 2021K's filenames (patient, consent, contact, admission) are in the current filename
        //        if (!SpecificationFileNames.Select(SpecificationFileName => File.Name.Contains(SpecificationFileName)).Contains(true))
        //        {
        //            return new FileValidationState() { IsValid = ValidState.Warning, Message = "File name not in expected list of file names" };
        //        }

        //        if (IsCommaDelimited() != true)
        //        {
        //            return new FileValidationState() { IsValid = ValidState.Error, Message = "CSV file not comma separated" };
        //        }


        //        return new FileValidationState() { IsValid = ValidState.Good, Message = "File passed validation checks" };
        //    }
        //}
    }
}
