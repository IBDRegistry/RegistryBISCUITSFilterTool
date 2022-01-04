using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripV3Consent.View
{
    public class OutputFileItem: AbstractFileItem<LoadedFile>
    {
        public OutputFileItem(LoadedFile file) : base(file)
        {

        }
    }
}
