using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.Model
{
    public class OnlyOneRecordExpectedException : Exception
    {
        public string NHSNumber;

        public string MessageToAppend;

        public override string ToString()
        {
            StringBuilder Message = new StringBuilder();
            Message.Append("Only one record was expected. ");

            if (NHSNumber is null) {
                Message.Append($"Patient {NHSNumber}. ");
            }

            return Message.ToString();
        }
    }
}
