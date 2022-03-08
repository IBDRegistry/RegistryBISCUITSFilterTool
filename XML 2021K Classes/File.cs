using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Specification
{
    [System.Xml.Serialization.XmlInclude(typeof(RegistryFile))]
    [System.Xml.Serialization.XmlInclude(typeof(NationalOptOutFile))]
    public class File
    {
        public string Name;

        public List<Field> Fields = new List<Field>();

        /// <summary>
        /// Is this file a registry file from 2021K
        /// </summary>
        public bool IsRegistryFile;

        /// <summary>
        /// Returns whether the file contains patient level identifiers i.e. NHS Number & Date of Birth
        /// This was designed with filtering out provenance_abc.csv in mind
        /// </summary>
        public bool IsPatientLevelFile { get {
                return Fields.Select(f => f.DataItemCode).Intersect(DataSubmissionSpecification.IdentifierCodes).SequenceEqual(DataSubmissionSpecification.IdentifierCodes); 
            } }

        /// <summary>
        /// Turns the specification file name into a simplified name without the extension or _abc e.g. patient_abc.csv => patient
        /// </summary>
        public string SimplifiedName
        {
            get => Name.Replace(@".*\", "").Replace(".csv", "");
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
