using System;
using System.Collections.Generic;
using System.Text;

namespace StripConsentModel.Model.Common
{
    public class Error
    {
        public string FilePath;
        public string ErrorDescription;

        public Error(string filePath, string errorDescription)
        {
            FilePath = filePath;
            ErrorDescription = errorDescription;
        }
    }

    public static class ErrorLogger
    {
        public static List<Error> Errors { get; private set; } = new List<Error>();

        public static void Add(string filePath, string errorDescription) => Errors.Add(new Error(filePath, errorDescription));
    }
}
