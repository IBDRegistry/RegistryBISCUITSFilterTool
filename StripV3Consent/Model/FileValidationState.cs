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

    public class FileValidationState
    {
        public ValidState IsValid;
        public string Message;
    }
}
