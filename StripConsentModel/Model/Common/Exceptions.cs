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

        public IEnumerable<Record> ProblematicRecordGroup;

        public override string ToString()
        {
            StringBuilder Message = new StringBuilder();
            Message.Append("Only one record was expected. ");

            if (!(NHSNumber is null)) {
                Message.Append($"Patient {NHSNumber}. ");
            }

            if (!(ProblematicRecordGroup is null)) {
                Message.Append($"There are {ProblematicRecordGroup.Count()} records from the {String.Join(" & ", ProblematicRecordGroup.Select(r => r.OriginalFile.Name))} files when there is only supposed to be 1 ");
            }

            return Message.ToString();
        }
    }
}
