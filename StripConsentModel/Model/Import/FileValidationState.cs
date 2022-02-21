using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StripV3Consent.Model
{
    public enum ValidState {
        None,
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
        public ValidState ValidState;
        public FileOrganisation Organisation;
        public string Message;
    }
}
