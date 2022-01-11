using StripV3Consent.Model;
using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using StripV3Consent.View;

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
            ImportFile[] ValidFiles = (sender as ObservableCollection<ImportFile>).Where(file => file.IsValid.IsValid != ValidState.Error & file.IsValid.IsValid != ValidState.None).ToArray();

            if (ValidFiles.Length > 0)
            {
                ExecuteButton.Enabled = true;
            } else
            {
                ExecuteButton.Enabled = false;
            }
        }

        private void OutputFilesChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<OutputFile> OutputFiles = sender as ObservableCollection<OutputFile>;

            if (OutputFiles.Count > 0)
            {
                SaveButton.Enabled = true;
            }
            else
            {
                SaveButton.Enabled = false;
            }
        }

        private void RemovedPatientsPanel_AllRecordSetsChanged(object sender, EventArgs e)
        {
            RecordSetGrouping AllRecordSetsGrouped = ((RemovedPatientsPanel)sender).AllRecordSets;

            if (AllRecordSetsGrouped != null)
            {
                DisplayKeptPatientsCheckbox.Enabled = true;
                DisplayRemovedPatientsCheckbox.Enabled = true;
            } else
            {
                DisplayKeptPatientsCheckbox.Enabled = false;
                DisplayRemovedPatientsCheckbox.Enabled = false;
            }
        }

        public static Predicate<T> Or<T>(IEnumerable<Predicate<T>> predicates)
        {
            return delegate (T item)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
                return false;
            };
        }

        private void DisplayCheckboxesChanged(object sender, EventArgs e)
        {
            //Swap from Func<RecordSet, bool> to Predicate<RecordSet>
            List<Tuple<CheckBox, Predicate<RecordSet>>> CheckboxSpecifiers = new List<Tuple<CheckBox, Predicate<RecordSet>>>()
            {
                new Tuple<CheckBox, Predicate<RecordSet>>(DisplayKeptPatientsCheckbox, rs => rs.IsConsentValid == true ),
                new Tuple<CheckBox, Predicate<RecordSet>>(DisplayRemovedPatientsCheckbox, rs => rs.IsConsentValid == false)
            };

            IEnumerable<Predicate<RecordSet>> Specifiers = CheckboxSpecifiers.Where(tuple => tuple.Item1.Checked == true).Select(tuple => tuple.Item2);

            Predicate<RecordSet> CombinedSpecifier = Or<RecordSet>(Specifiers);

            RemovedPatientsPanel.Specifier = CombinedSpecifier;
        }

        private void ExecuteButton_Click(object sender, EventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            ImportFile[] StartingFiles = DropFilesHerePanel.FileList.Files.Where(File => File.IsValid.IsValid == ValidState.Good || File.IsValid.IsValid == ValidState.Warning).ToArray();
            RecordSetGrouping RecordsGroupedByPatient = new Model.RecordSetGrouping(StartingFiles);

            RemovedPatientsPanel.AllRecordSets = RecordsGroupedByPatient;

            OutputFile[] OutputFiles = RecordsGroupedByPatient.SplitBackUpIntoFiles(RecordSet => RecordSet.IsConsentValid == true);

            LoadedFilesPanel.FileList.Files.Clear();
            LoadedFilesPanel.FileList.AddRange(OutputFiles);

            
        }

        private void ExecuteWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            
        }

        private void ExecuteWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void ExecuteWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }


    }
}
