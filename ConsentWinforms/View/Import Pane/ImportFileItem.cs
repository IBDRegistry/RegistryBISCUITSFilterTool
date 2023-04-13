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
    public class ImportFileItem : AbstractFileItem<ImportFile>
    {
        public IconWithSubIcon ValidationPictureBox;

        public ImportFileItem(ImportFile file) : base(file)
        {
        }

        public void ReCheckValidation()
        {
            ValidationPictureBox.Model = base.File.IsValid;
        }

        public override void DrawContents()
        {
            base.DrawContents();

            ValidationPictureBox = new IconWithSubIcon()
            {
                Model = base.File.IsValid
            };

            this.Controls.Add(ValidationPictureBox);
        }
    }


    public class IconWithSubIcon : PictureBox
    {
        private FileValidationState model;

        public FileValidationState Model
        {
            get => model;
            set
            {
                model = value;
                Draw();
            }
        }

        private const int width = 32;
        private const int height = width;

        private PictureBox LittleIcon = new PictureBox()
        {
            Width = width / 2,
            Height = height /2,
            Location = new Point(width / 2, height / 2),    //Anchoring to the bottom right just doesn't seem to work so in the meantime just position the top left to the middle
            //Anchor = AnchorStyles.Bottom | AnchorStyles.Right,
            SizeMode = PictureBoxSizeMode.StretchImage,
            BackColor = Color.Transparent
        };

        public IconWithSubIcon()
        {
            Width = width;
            Height = height;
            SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(LittleIcon);
        }

        private void Draw()
        {
            switch (Model.Organisation)
            {
                case FileOrganisation.NHS:
                    Image = ConsentWinforms.Properties.Resources.NationalOptOut;
                    break;
                case FileOrganisation.Registry:
                    Image = ConsentWinforms.Properties.Resources.ibdregistry;
                    break;
                case FileOrganisation.Unknown:
                    Image = ConsentWinforms.Properties.Resources.help_question_mark.ToBitmap();
                    break;
            }

            Icon ValidImage = null;
            switch (Model.ErrorLevel)
            {
                case ValidState.Good:
                    ValidImage = ConsentWinforms.Properties.Resources.good;
                    break;
                case ValidState.Warning:
                    ValidImage = ConsentWinforms.Properties.Resources.warning;
                    break;
                case ValidState.Error:
                    ValidImage = ConsentWinforms.Properties.Resources.error;
                    break;
            }

            LittleIcon.Image = ValidImage?.ToBitmap();


            Control[] SetValidationToolTipOn = { this, LittleIcon };
            foreach(Control ctrl in SetValidationToolTipOn)
			{
                new ToolTip() { ShowAlways = true }.SetToolTip(ctrl, string.Join(Environment.NewLine, Model.Messages));
            }
            
        }
        
    }
}
