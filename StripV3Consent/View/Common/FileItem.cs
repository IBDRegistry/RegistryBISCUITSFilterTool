using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace StripV3Consent.View
{
    public abstract class AbstractFileItem<FileType> : FlowLayoutPanel where FileType : DataFile
    {
        private FileType file;

        public PictureBox FileIcon;
        private Button closeButton;

        private const int InteriorSize = 32;

        public Button CloseButton => closeButton;


        public FileType File
        {
            get => file;
            set
            {
                file = value;
                DrawContents();
            }
        }

        public AbstractFileItem(FileType file)
        {
            CustomizeControl();
            File = file;
        }

        private void CustomizeControl()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.FlowDirection = FlowDirection.LeftToRight;
            this.WrapContents = false;
            this.AutoSize = true;
        }

        public virtual void DrawContents()
        {
            FileIcon = new PictureBox()
            {
                Image = Icon.ExtractAssociatedIcon(file.Path).ToBitmap(),
                Width = InteriorSize,
                Height = InteriorSize
            };
            Icon test = Icon.ExtractAssociatedIcon(file.Path);
            this.Controls.Add(FileIcon);

            Label FileNameLabel = new Label()
            {
                Text = file.Name,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoEllipsis = true
            };
            this.Controls.Add(FileNameLabel);

            closeButton = new Button()
            {
                Text = "X",
                Width = InteriorSize,
                Height = InteriorSize,
            };
            
        }
    }
}
