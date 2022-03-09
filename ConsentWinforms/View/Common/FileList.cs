using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private ObservableRangeCollection<DataFileType> _files = new ObservableRangeCollection<DataFileType>();
        public ObservableRangeCollection<DataFileType> Files
		{
            get => _files;
            set
			{
                _files = value;
                Files.CollectionChanged += Files_CollectionChanged;
                //Files_CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
		}

        public FileList()
		{
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
            Files.AddRange(items);
        }

        #endregion

        /// <summary>
        /// Used for setting if you want a remove button to appear to the right of each FileItem
        /// </summary>
        public bool RemoveButtons = true;

        /// <summary>
        /// A panel that appears when there are no FileItems in the FileList and sticks to the bottom of the list as more are added/removed
        /// </summary>
        public readonly Panel BottomPanel = new Panel() { 
            Dock = DockStyle.Top,
            AutoSize = true,
            Height = 120
        };

        private void CustomizeControl()
        {
            FlowDirection = FlowDirection.TopDown;
            Dock = DockStyle.Fill;
            AutoScroll = true;
            WrapContents = false;
            BorderStyle = BorderStyle.FixedSingle;

            Controls.Add(BottomPanel);
        }

        private void Files_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
			switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (object NewItem in e.NewItems)
					{
                        AddItem((DataFileType)NewItem);
                    }
                    
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RedrawList();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    DataFileType FileToRemove = e.OldItems.Cast<DataFileType>().First();
                    RemoveItem(FileToRemove);
                    break;
            }
        }


        private void AddItem(DataFileType File)
        {
            FileItemType NewEntry = (FileItemType)Activator.CreateInstance(typeof(FileItemType), new object[] { File });
            

            if (RemoveButtons)
            {
                NewEntry.CloseButton.Click += ItemCloseButton_Click;
                NewEntry.Controls.Add(NewEntry.CloseButton);
            }

            this.Controls.Add(NewEntry);

            int IndexOfBottomPanel = Controls.GetChildIndex(BottomPanel);
            int IndexOfNewControl = Controls.GetChildIndex(NewEntry);
            Controls.SetChildIndex(NewEntry, IndexOfBottomPanel);
            Controls.SetChildIndex(BottomPanel, IndexOfNewControl);
        }

        private void RemoveItem(DataFileType File)
        {
            FileItemType FileItemToRemove = Controls.Cast<FileItemType>().Where(FileItem => FileItem.File == File).First();
            Controls.Remove(FileItemToRemove);
        }
        
        private void RedrawList()
        {
            this.Controls.Clear();
            foreach(DataFileType File in Files)
            {
                AddItem(File);
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

