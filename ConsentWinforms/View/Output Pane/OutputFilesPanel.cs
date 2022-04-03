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
        public FileList<OutputFileItem, OutputFile> FileList = new OutputFileList()
        {
            RemoveButtons = false
        };

        public OutputFilesPanel()
        {
            this.Controls.Add(FileList);

            FileList.BottomPanel.AutoSize = true;
        }


        private class OutputFileList: FileList<OutputFileItem, OutputFile>
		{
			protected override OutputFileItem CreateItemInstance(OutputFile DataFile)
			{
				switch (DataFile.SpecificationFile.IsPatientLevelFile)
				{
                    case true:
                        return new PatientOutputFileItem(DataFile);
                    case false:
                        return new NonPatientOutputFileItem(DataFile);
                    default:
                        return null;
				}
			}
		}
    }
}
