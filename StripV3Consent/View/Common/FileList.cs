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
        private ObservableCollection<DataFileType> _files = new ObservableCollection<DataFileType>();
        public ObservableCollection<DataFileType> Files
		{
            get => _files;
            set
			{
                _files = value;
                Files.CollectionChanged += Files_CollectionChanged;
            }
		}

        public FileList(ObservableCollection<DataFileType> FileListToUse = null) {
            if (!(FileListToUse is null))
                Files = FileListToUse;

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

        #endregion

        public bool RemoveButtons = true;

        private void CustomizeControl()
        {
            FlowDirection = FlowDirection.TopDown;
            Dock = DockStyle.Fill;
            AutoScroll = true;
            WrapContents = false;
        }

        private void Files_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddItem((DataFileType)e.NewItems[0]);
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

        public bool IsFileAlreadyInList(ImportFile File)
        {
            //This line looks a bit crazy but the point of it is two create an IEnumerable of 1.The file calling (File) and 2.Other files that are have the 'already imported' error
            //We need to do 1. otherwise the function always returns true as it will detect itself in the list when doing .Contains()
            //We also need to do 2. otherwise scenarios with multiple files from the same specification file get buggy as they and the method never gives them all the error until only one of them is left
            //We need to get the information from 2. from the Controls collection not the Files ObservableCollection as calling Files.Where(file => file.IsValid.Message =="Already imported") calls this method in IsValid resulting in a StackOverflow
            //By using the one stored in the UI it acts as a sort of cache so no circular references or StackOverflow.
            IEnumerable<ImportFile> FilesToExclude = (new ImportFile[] { File }).Union(Controls.Cast<ImportFileItem>().Where(control => control.ValidationPictureBox.Model.Message == "File already imported").Select(control => control.File));
            
            IEnumerable <ImportFile> ImportFiles = Files.Cast<ImportFile>().Except(FilesToExclude);
            IEnumerable<Specification.File> SpecificationFiles = ImportFiles.Select(i => i.SpecificationFile);
            return SpecificationFiles.Contains(File.SpecificationFile);
        }

        public void ReCheckFileCollisions(ImportFile File)
        {
            IEnumerable<ImportFileItem> OtherFilesWithSameSpecificationFile = Controls.Cast<ImportFileItem>().Where(i => i.File.SpecificationFile == File.SpecificationFile);
            foreach (ImportFileItem CurrentOtherFile in OtherFilesWithSameSpecificationFile.ToArray())
            {
                CurrentOtherFile.ReCheckValidation();
            }
        }

        private void AddItem(DataFileType File)
        {
            if (typeof(DataFileType) == typeof(ImportFile))//Set collision detection callback
            {
                ((ImportFile)(DataFile)File).IsSpecificationFileAlreadyImported = IsFileAlreadyInList;
            }

            FileItemType NewEntry = (FileItemType)Activator.CreateInstance(typeof(FileItemType), new object[] { File });
            

            if (RemoveButtons)
            {
                NewEntry.CloseButton.Click += ItemCloseButton_Click;
                NewEntry.Controls.Add(NewEntry.CloseButton);
            }

            this.Controls.Add(NewEntry);
        }

        private void RemoveItem(DataFileType File)
        {
            FileItemType FileItemToRemove = Controls.Cast<FileItemType>().Where(FileItem => FileItem.File == File).First();
            Controls.Remove(FileItemToRemove);
            ReCheckFileCollisions((ImportFile)(DataFile)FileItemToRemove.File);
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

