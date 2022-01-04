using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    class ImportFileItem : AbstractFileItem<ImportFile>
    {
        public ImportFileItem(ImportFile file) : base(file)
        {
        }

        public override void DrawContents()
        {
            base.DrawContents();

            Icon ValidationImage = null;
            FileValidationState FileValidation = base.File.IsValid;
            switch (FileValidation.IsValid)
            {
                case ValidState.Good:
                    ValidationImage = Properties.Resources.good;
                    break;
                case ValidState.Warning:
                    ValidationImage = Properties.Resources.warning;
                    break;
                case ValidState.Error:
                    ValidationImage = Properties.Resources.error;
                    break;
            }

            PictureBox ValidationPictureBox = new PictureBox()
            {
                Image = ValidationImage.ToBitmap(),
                SizeMode = PictureBoxSizeMode.AutoSize
            };

            this.Controls.Add(ValidationPictureBox);


            new ToolTip() { ShowAlways = true }.SetToolTip(ValidationPictureBox, FileValidation.Message);



        }
    }
}
