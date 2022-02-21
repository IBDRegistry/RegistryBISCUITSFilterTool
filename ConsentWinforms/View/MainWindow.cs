using StripV3Consent.Model;
using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Security.Principal;

namespace StripV3Consent.View
{
    public partial class MainWindow : Form
    {
        private ConsentToolModel Model = new ConsentToolModel();

        public MainWindow()
        {
            InitializeComponent();
            
            LoadedFilesPanel.FileList.Files.CollectionChanged += OutputFilesChanged;

            DropFilesHerePanel.FileList.Files = Model.InputFiles;
            RemovedPatientsPanel.AllRecordSets = Model.Patients;
            LoadedFilesPanel.FileList.Files = Model.OutputFiles;

			CheckIfAdministrator();
        }

        private void CheckIfAdministrator()
        {
            if (IsAdministrator())
            {
                MessageBox.Show(text:
@"This program is being run in administrator mode, this is known to cause issues with drag and drop in Windows systems implementing UAC (Vista and newer)
To resolve this either drag and drop from Windows Explorer while running it as administrator, or run the program as a limited user

You can contact your IT support for help with this issue",
                                caption:"Warning: Administrator Privileges",
                                buttons: MessageBoxButtons.OK,
                                icon: MessageBoxIcon.Warning);
            }
        }

        private bool IsAdministrator()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog SelectDialog = new CommonOpenFileDialog();
      
            SelectDialog.InitialDirectory = DropFilesHerePanel.MostCommonSourceDirectory;
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
            ObservableCollection<RecordSet> AllRecordSetsGrouped = ((RemovedPatientsPanel)sender).AllRecordSets;

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
    }
}
