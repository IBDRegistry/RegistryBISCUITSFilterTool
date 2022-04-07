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
            
            DropFilesHerePanel.FileList.Files = Model.InputFiles;
            RemovedPatientsPanel.AllRecordSets = Model.Patients;
            LoadedFilesPanel.FileList.Files = Model.OutputFiles;

            LoadedFilesPanel.FileList.Files.CollectionChanged += OutputFilesChanged;
            Model.InputFilesChanged += UpdateBlankFilesRemovedLabel;

            CheckIfAdministrator();
        }

        private void UpdateBlankFilesRemovedLabel(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            IEnumerable<Specification.File> SpecificationFilesInInput = Model.InputFiles.GroupBy
                <ImportFile, Specification.File, Specification.File>
                (file => file.SpecificationFile,    //group by specification file
                (SpecificationFile, ImportFileIEnumerable) => SpecificationFile //output specification file from IGrouping
                );

            IEnumerable<Specification.File> SpecificationFilesInOutput = Model.OutputFiles.Select(OutFile => OutFile.SpecificationFile);

            List<Specification.File> SpecificationFilesThatDidntMakeIt = SpecificationFilesInInput.Except(SpecificationFilesInOutput).ToList();
            SpecificationFilesThatDidntMakeIt.RemoveAll(element => element is null);

            List<ImportFile> InputFilesThatDidntMakeIt = Model.InputFiles.Where(inputFile => SpecificationFilesThatDidntMakeIt.Contains(inputFile.SpecificationFile)).ToList();

            

            const string BlankFilesRemovedLabelName = "BlankFilesRemovedLabel";

            string LabelText = "";
            if (InputFilesThatDidntMakeIt.Count() != 0)  {
                LabelText = $"{InputFilesThatDidntMakeIt.Count()} files were excluded from the output: {Environment.NewLine}{string.Join(Environment.NewLine, InputFilesThatDidntMakeIt.Select(file => $"-{file.Name}").ToArray())}";
            }
            
            Control ExistingLabel = LoadedFilesPanel.FileList.BottomPanel.Controls.Cast<Control>().Where(ctrl => ctrl.Name == BlankFilesRemovedLabelName).FirstOrDefault();
            if (ExistingLabel == null)
            {
                Label NewLabel = new Label()
                {
                    Name = BlankFilesRemovedLabelName,
                    AutoSize = true,
                    Text = LabelText
                };
                LoadedFilesPanel.FileList.BottomPanel.Controls.Add(NewLabel);
            } else
            {
                ((Label)ExistingLabel).Text = LabelText;
            }

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

        private bool CheckNationalOptOut()
		{
            const string NOOFileNameInSpec = ".dat";
            //enter if either true & false or false & true
            bool NOOChecked = ConsentToolModel.EnableNationalOptOut;
            bool NOOFilePresent = Model.InputFiles.Where(i => i.SpecificationFile.Name.Contains(NOOFileNameInSpec)).Count() > 0;
            if (NOOChecked != NOOFilePresent)
			{
                string MessageBoxText = null;
                if (NOOChecked == true && NOOFilePresent == false)
                {
                    MessageBoxText = "You have ticked the national data opt-out compliancy checkbox but not loaded a relevant file. This means that all unconsented records will be removed in the output files";
                    DialogResult MessageBoxResult = MessageBox.Show(
                        text: $"{MessageBoxText}. Do you wish to continue?",
                        caption: "National data opt-out mismatch",
                        buttons: MessageBoxButtons.YesNo,
                        icon: MessageBoxIcon.Warning
                    );
                    if (MessageBoxResult == DialogResult.Yes)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                } else if (NOOChecked == false && NOOFilePresent == true)
                {
                    DialogResult MessageBoxResult = MessageBox.Show(
                        text: "You have loaded a national data opt-out file but have not ticked the box to turn on the NDOO compliancy function.\nPlease switch the functionality on using the on-screen checkbox before downloading your outputs.",
                        caption: "National data opt-out mismatch",
                        buttons: MessageBoxButtons.OK,
                        icon: MessageBoxIcon.Error
                    );
                    return false;
                } else
				{
                    return true;
				}
			} else
			{
                return true;
			}

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (CheckNationalOptOut() != true)
			{
                return;
			}
            CommonOpenFileDialog SelectDialog = new CommonOpenFileDialog();

            if (DropFilesHerePanel.MostCommonSourceDirectory != null)
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

                List<string> FilesToWrite = LoadedFilesPanel.FileList.Files.Select(OutputFile => OutputFile.StringOutput()).ToList<string>();

                foreach(OutputFile OutFile in LoadedFilesPanel.FileList.Files)
                {
                    //Remove wildcard and append _Processed to filename
                    string FileName = OutFile.SpecificationFile.Name.Replace(@".*\", "").Replace(".csv", "_Processed.csv");
                    string OutPath = OutputFolder + FileName;
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(OutPath))
                        {
                            writer.Write(OutFile.StringOutput());
                        }
                    }
                    catch (Exception ex)
					{
                        MessageBox.Show($"There was an {ex.ToString()} while writing file {FileName} to disk, this will not have been completed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        continue;
					}
                }

                //Open explorer window to show results
                StartExplorer(OutputFolder);

            }
        }

        //Process.Start is slow so putting it into an async void "Fire and Forget" method means we can prevent it from blocking the UI thread for about 7 seconds
        private async void StartExplorer(string FolderPath)
		{
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
            {
                Arguments = FolderPath,
                FileName = "explorer.exe"
            };

            System.Diagnostics.Process.Start(startInfo);
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

        private void MainWindow_Load(object sender, EventArgs e)
        {
            ConsentToolModel.EnableNationalOptOut = CheckOptOutFile.Checked;   //Set designer value as default value, currently this is true
        }


        private void CheckOptOutFile_CheckedChanged(object sender, EventArgs e)
        {
            ConsentToolModel.EnableNationalOptOut = CheckOptOutFile.Checked;
        }
    }
}
