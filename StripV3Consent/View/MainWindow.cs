using StripV3Consent.Model;
using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;

namespace StripV3Consent
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            DropFilesHerePanel.FileList.Files.CollectionChanged += ImportFilesChanged;
            LoadedFilesPanel.FileList.Files.CollectionChanged += OutputFilesChanged;
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            RecordSetGrouping RecordsGroupedByPatient = new Model.RecordSetGrouping(DropFilesHerePanel.FileList.Files.Where(File => File.IsValid.IsValid == ValidState.Good || File.IsValid.IsValid == ValidState.Warning).ToArray());

            List<RecordSet> RemovedRecords = RecordsGroupedByPatient.RecordSets.Where(RecordSet => RecordSet.IsConsentValid == false).ToList<RecordSet>();

            RecordSetGrouping RecordSetsWithConsent = (RecordSetGrouping)RecordsGroupedByPatient.RecordSets.Where(RecordSet => RecordSet.IsConsentValid == true).ToList<RecordSet>();

            OutputFile[] OutputFiles = RecordSetsWithConsent.SplitBackUpIntoFiles();

            RemovedPatientsPanel.RemovedRecords = new BindingList<RecordSet>(RemovedRecords);

            LoadedFilesPanel.FileList.AddRange(OutputFiles);

            
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog SelectDialog = new CommonOpenFileDialog();

            string MostCommonSourceDirectory = LoadedFilesPanel.FileList.Files.Select(
                                                                            OutputFile => Path.GetDirectoryName(OutputFile.Path))
                                                                                .GroupBy(
                                                                                  Path => Path,
                                                                                  (FilePath, DF) => new {
                                                                                                         path = FilePath,
                                                                                                         NumberOf = DF.Count()
                                                                                                        }
                                                                                  ).OrderBy(
                                                                                       AnonObject => AnonObject.NumberOf).First().path;



            SelectDialog.InitialDirectory = MostCommonSourceDirectory;
            SelectDialog.IsFolderPicker = true;
            if (SelectDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string OutputFolder = SelectDialog.FileName + "\\Processed\\";

                if (Directory.Exists(OutputFolder))
                {
                    DialogResult output = MessageBox.Show("Output folder already exists, do you want to overwrite it?", "Output folder already exists", MessageBoxButtons.YesNo);
                    if (output == DialogResult.Yes)
                    {
                        try
                        {
                            Directory.Delete(OutputFolder, true);
                        } catch (Exception ex)
                        {
                            MessageBox.Show(
                                            $"An error occured while attempting to delete {OutputFolder}, " +
                                            $"\"{ex.Message}\"", "Error while trying to delete folder", 
                                            MessageBoxButtons.OK, 
                                            MessageBoxIcon.Error
                                            );
                            return;
                        }
                        
                    } else
                    {
                        return;
                    }
                }
                Directory.CreateDirectory(OutputFolder);

                List<string> FilesToWrite = LoadedFilesPanel.FileList.Files.Select(OutputFile => OutputFile.RepackIntoString()).ToList<string>();

                foreach(OutputFile OutFile in LoadedFilesPanel.FileList.Files)
                {
                    string OutPath = OutputFolder + OutFile.Name;
#warning add try catch for StreamWriter I/O
                    using (StreamWriter writer = new StreamWriter(OutPath))
                    {
                        writer.Write(OutFile.RepackIntoString());
                    }
                }

                //Open explorer window to show results
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
                {
                    Arguments = OutputFolder,
                    FileName = "explorer.exe"
                };

                System.Diagnostics.Process.Start(startInfo);

            }
        }

        private void ImportFilesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var SourceCollection = sender as ObservableCollection<ImportFile>;

            if (SourceCollection.Count > 0)
            {
                ExecuteButton.Enabled = true;
            } else
            {
                ExecuteButton.Enabled = false;
            }
        }

        private void OutputFilesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var OutputCollection = sender as ObservableCollection<OutputFile>;

            if (OutputCollection.Count > 0)
            {
                SaveButton.Enabled = true;
            }
            else
            {
                SaveButton.Enabled = false;
            }
        }

    }
}
