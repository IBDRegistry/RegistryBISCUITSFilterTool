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
                                                       where FileItemType : IFileItem<DataFileType>
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
            }
		}

        public FileList()
		{
            FlowDirection = FlowDirection.TopDown;
            Dock = DockStyle.Fill;
            AutoScroll = true;
            WrapContents = false;
            BorderStyle = BorderStyle.FixedSingle;

            Controls.Add(BottomPanel);
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
            Height = 120,
        };

        private void Files_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
			switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (object NewItem in e.NewItems)
					{
                        Invoke((Action)(() => AddItem((DataFileType)NewItem)));
                    }
                    
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Invoke((Action)(() => RedrawList()));
                    
                    break;
                case NotifyCollectionChangedAction.Remove:
                    DataFileType FileToRemove = e.OldItems.Cast<DataFileType>().First();
                    Invoke((Action)(() => RemoveItem(FileToRemove)));
                    break;
            }
        }

        protected virtual FileItemType CreateItemInstance(DataFileType DataFile)
		{
            return (FileItemType)Activator.CreateInstance(typeof(FileItemType), new object[] { DataFile });
        }

        private void AddItem(DataFileType File)
        {
            FileItemType NewEntry = CreateItemInstance(File);



            if (RemoveButtons)
            {
                NewEntry.CloseButton.Click += ItemCloseButton_Click;
                NewEntry.Controls.Add(NewEntry.CloseButton);
            }

            Controls.Add(NewEntry.control);

            int IndexOfBottomPanel = Controls.GetChildIndex(BottomPanel);
            int IndexOfNewControl = Controls.GetChildIndex(NewEntry.control);
            Controls.SetChildIndex(NewEntry.control, IndexOfBottomPanel);
            Controls.SetChildIndex(BottomPanel, IndexOfNewControl);
        }

        private void RemoveItem(DataFileType File)
        {
            FileItemType FileItemToRemove = Controls.Cast<FileItemType>().Where(FileItem => FileItem.File == File).First();
            Controls.Remove(FileItemToRemove.control);
        }
        
        private void RedrawList()
        {
            IFileItem<DataFileType>[] ControlsToRemove = Controls.Cast<Control>().Where(ctrl => ctrl is IFileItem<DataFileType>).Select(ctrl => (IFileItem<DataFileType>)ctrl).ToArray();
            foreach(FileItemType fileItem in ControlsToRemove)
            {
                Controls.Remove(fileItem.control);
            }
            foreach(DataFileType File in Files)
            {
                AddItem(File);
            }
        }

        private async void ItemCloseButton_Click(object sender, EventArgs e)
        {
            Button CloseButtonClicked = (Button)sender;
            IFileItem<DataFileType> Entry = (AbstractFileItem<DataFileType>)CloseButtonClicked.Parent;
            await System.Threading.Tasks.Task.Run(() => Files.Remove(Entry.File));
        }

        public override string ToString()
        {
            return $"FileList of {typeof(FileItemType).ToString()}";
        }

    }
}

