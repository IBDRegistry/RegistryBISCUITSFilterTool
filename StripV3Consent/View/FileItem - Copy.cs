using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace StripV3Consent.View
{
    public class FileItem : FlowLayoutPanel, IFileItem<DataFile>
    {
        private DataFile file;

        public PictureBox FileIcon;
        private Button closeButton;

        public Button CloseButton => closeButton;

        public event EventHandler FileChanged;

        public DataFile File
        {
            get => file;
            set
            {
                file = value;
                FileChanged.Invoke(this, new EventArgs());
            }
        }
        public FileItem() { }

        public FileItem(DataFile file)
        {
            FileChanged += DrawContents;
            CustomizeControl();
            File = file;
        }

        private void CustomizeControl()
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            this.FlowDirection = FlowDirection.LeftToRight;
            this.AutoSize = true;
        }

        private void DrawContents(object sender, EventArgs e)
        {
            FileIcon = new PictureBox()
            {
                Image = Icon.ExtractAssociatedIcon(file.Path).ToBitmap(),
                SizeMode = PictureBoxSizeMode.AutoSize
            };
            this.Controls.Add(FileIcon);

            Label FileNameLabel = new Label()
            {
                Text = file.Name,
                TextAlign = ContentAlignment.MiddleLeft
            };
            this.Controls.Add(FileNameLabel);

            closeButton = new Button()
            {
                Text = "X",
                Width = 20,
                Height = 20,
            };
            
        }
    }
}
