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
        public FileList<IFileItem<OutputFile>, OutputFile> FileList = new OutputFileList()
        {
            RemoveButtons = false
        };

        public OutputFilesPanel()
        {
            this.Controls.Add(FileList);

            FileList.BottomPanel.AutoSize = true;
        }


        private class OutputFileList: FileList<IFileItem<OutputFile>, OutputFile>
		{
			protected override IFileItem<OutputFile> CreateItemInstance(OutputFile DataFile)
			{
				switch (DataFile.SpecificationFile.IsPatientLevelFile)
				{
                    case true:
                        return new PatientOutputFileItem((RepackingOutputFile)DataFile);
                    case false:
                        return new NonPatientOutputFileItem((DirectOutputFile)DataFile);
                    default:
                        return null;
				}
			}
		}
    }
}
