using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StripConsentModel.Model.Import
{
    public class ImportBatch
    {
        public ImportBatch(string batchFolderPath)
        {
            BatchFolderPath = batchFolderPath;
        }

        public string BatchFolderPath { get; private set; }
        public List<ImportFile> Files { get; private set; } = new List<ImportFile>();

        public void Add(ImportFile file) => Files.Add(file);
        public void Remove(ImportFile file) => Files.Remove(file);


    }
}
