using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripV3Consent.Model
{
    public enum ValidState {
        Good,
        Warning,
        Error
    }

    public enum FileOrganisation
    {
        Registry,
        NHS,
        Unknown
    }

    public class FileValidationState
    {
        public ValidState IsValid;
        public FileOrganisation Organisation;
        public string Message;
    }
}
