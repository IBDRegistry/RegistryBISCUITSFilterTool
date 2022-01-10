using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StripV3Consent.Model;

namespace StripV3Consent.View
{
    class DropFilesPanel : Panel
    {
        public FileList<ImportFileItem, ImportFile> FileList = new FileList<ImportFileItem, ImportFile>();

        public DropFilesPanel()
        {
            this.AllowDrop = true;
            this.DragEnter += DropFiles_DragEnter;
            this.DragDrop += DropFiles_DragDrop;
        }

        private void DropFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private  void DropFiles_DragDrop(object sender, DragEventArgs e)
        {
            string[] InputPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            string[] FilePaths = InputPaths.Where(path => !(File.GetAttributes(path) == FileAttributes.Directory | File.GetAttributes(path) == FileAttributes.Device)).ToArray(); //Prevent doctors from trying to drag and drop folders, devices and all sorts of nonsense
            this.Controls.Clear();
            this.Controls.Add(FileList);
            FileList.AddRange(FilePaths.Select<string, ImportFile>(Path => new ImportFile(Path)).ToArray());
        }


    }
}
