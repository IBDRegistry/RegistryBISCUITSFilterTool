using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using StripV3Consent.Model;

namespace StripV3Consent.View
{
    public class FileList<FileItemType, DataFileType>: FlowLayoutPanel
                                                       where FileItemType : AbstractFileItem<DataFileType> 
                                                       where DataFileType : DataFile
    {

        #region files
        public ObservableCollection<DataFileType> Files = new ObservableCollection<DataFileType>();

        public FileList(DataFileType[] files) : this()
        {
            AddRange(files);
        }

        public FileList() {
            Files.CollectionChanged += RedrawList;
            CustomizeControl(); 
        }

        public DataFileType this[int index]
        {
            get
            {
                return Files[index];
            }
            set
            {
                Files[index] = value;
            }
        }

        public void AddRange(DataFileType[] items)
        {
            foreach (DataFileType item in items)
            {
                Files.Add(item);
            }
        }


        public static explicit operator DataFileType[](FileList<FileItemType, DataFileType> me) => me.Files.ToArray();
        public static explicit operator FileList<FileItemType, DataFileType>(DataFileType[] array) => new FileList<FileItemType, DataFileType>(array);

        #endregion

        public bool RemoveButtons = true;

        private void CustomizeControl()
        {
            FlowDirection = FlowDirection.TopDown;
            Dock = DockStyle.Fill;
            AutoScroll = true;
            WrapContents = false;
        }
        private void RedrawList(object sender, EventArgs e)
        {
            this.Controls.Clear();
            foreach(DataFile File in Files)
            {
                FileItemType NewEntry = (FileItemType)Activator.CreateInstance(typeof(FileItemType), new object[] { File });

                if (RemoveButtons)
                {
                    NewEntry.CloseButton.Click += ItemCloseButton_Click;
                    NewEntry.Controls.Add(NewEntry.CloseButton);
                }
                this.Controls.Add(NewEntry);


            }
        }

        private void ItemCloseButton_Click(object sender, EventArgs e)
        {
            Button CloseButtonClicked = (Button)sender;
            FileItemType Entry = (FileItemType)CloseButtonClicked.Parent;
            Files.Remove(Entry.File);
        }

        public override string ToString()
        {
            return $"FileList of {typeof(FileItemType).ToString()}";
        }

    }
}

