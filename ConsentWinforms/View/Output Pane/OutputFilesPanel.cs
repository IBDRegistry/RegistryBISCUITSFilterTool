using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StripV3Consent.Model;

namespace StripV3Consent.View
{
    class OutputFilesPanel : Panel
    {
        public FileList<OutputFileItem, OutputFile> FileList = new FileList<OutputFileItem, OutputFile>()
        {
            RemoveButtons = false
        };

        public OutputFilesPanel()
        {
            this.Controls.Add(FileList);
        }


    }
}
