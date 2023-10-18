using StripConsentModel.Model.Import;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace StripV3Consent.Model
{
    public class ImportBatchList : List<ImportBatch>
    {
        private static ImportBatch GetExistingBatchForFile(IEnumerable<ImportBatch> ImportBatches, ImportFile file) => ImportBatches.FirstOrDefault(batch => batch.BatchFolderPath.Equals(System.IO.Path.GetDirectoryName(file.FilePath)));

        public void ImportFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ActionAddedFiles(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    ActionRemovedFiles(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    break;
                case NotifyCollectionChangedAction.Replace:
                    ActionRemovedFiles(e.OldItems);
                    ActionAddedFiles(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    //do nothing
                    break;
            }

        }
        private void ActionAddedFiles(System.Collections.IList AddedFiles)
        {
            foreach (var newFile in AddedFiles)
            {
                var newImportFile = (ImportFile)newFile;

                var existingBatch = GetExistingBatchForFile(this, newImportFile);
                if (existingBatch != null)
                {
                    existingBatch.Add(newImportFile);
                    newImportFile.Batch = existingBatch;
                }
                else
                {
                    var batchFolder = System.IO.Path.GetDirectoryName(newImportFile.FilePath);
                    var newImportBatch = new ImportBatch(batchFolder);
                    newImportBatch.Add(newImportFile);
                    newImportFile.Batch = newImportBatch;
                    Add(newImportBatch);
                }
            }
        }
        private void ActionRemovedFiles(System.Collections.IList RemovedFiles)
        {
            foreach (var file in RemovedFiles)
            {
                var removedFile = (ImportFile)file;
                var batchOfRemovedFile = GetExistingBatchForFile(this, removedFile);
                batchOfRemovedFile.Remove(removedFile);
            }
        }
    }
}