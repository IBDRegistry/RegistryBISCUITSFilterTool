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
        private ObservableCollection<DataFileType> files = new ObservableCollection<DataFileType>();
        public DataFileType[] Files
        {
            get => files.ToArray();
            set
            {
                files = new ObservableCollection<DataFileType>(value);
                files.CollectionChanged += RedrawList;
                RedrawList(files, new EventArgs());
            }
        }

        public FileList(DataFileType[] files)
        {
            Files = files;

            CustomizeControl();
        }
        public FileList() { CustomizeControl(); }

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


        public void Add(DataFileType item) => files.Add(item);

        public void AddRange(DataFileType[] items)
        {
            foreach (DataFileType item in items)
            {
                Add(item);
            }
        }

        public void Clear() => files.Clear();

        public bool Remove(DataFileType item) => files.Remove(item);


        public static implicit operator DataFileType[](FileList<FileItemType, DataFileType> me) => me.Files.ToArray();
        public static implicit operator FileList<FileItemType, DataFileType>(DataFileType[] array) => new FileList<FileItemType, DataFileType>(array);


        private void CustomizeControl()
        {
            FlowDirection = FlowDirection.TopDown;
            files.CollectionChanged += RedrawList;
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
                NewEntry.CloseButton.Click += ItemCloseButton_Click;
                NewEntry.Controls.Add(NewEntry.CloseButton);
                this.Controls.Add(NewEntry);

            }
        }

        private void ItemCloseButton_Click(object sender, EventArgs e)
        {
            Button CloseButtonClicked = (Button)sender;
            FileItemType Entry = (FileItemType)CloseButtonClicked.Parent;
            Remove(Entry.File);
        }

        public override string ToString()
        {
            return $"FileList of {typeof(FileItemType).ToString()}";
        }

    }
}

