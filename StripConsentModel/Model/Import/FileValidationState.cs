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
		public ValidState ErrorLevel;

        /// <summary>
        /// Replaced "Nothing" with "Good" and "Good" with "Error" but not "Warning" with "Good"
        /// </summary>
        /// <param name="ProspectiveNewErrorLevel"></param>
        public void IncreaseErrorLevelIfStronger(ValidState ProspectiveNewErrorLevel)
		{
            if (ErrorLevel < ProspectiveNewErrorLevel)
                ErrorLevel = ProspectiveNewErrorLevel;
        }

        public FileOrganisation Organisation;
        public List<string> Messages = new List<string>();

		
	}
}
