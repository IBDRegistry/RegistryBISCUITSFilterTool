﻿using StripV3Consent.Model;
using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Timers;
using ConsentWinforms.View.MainWindow;

namespace StripV3Consent.View
{
    public partial class MainWindow : LockableForm
    {
        private ConsentToolModel Model = new ConsentToolModel();

        private ProgressForm progress;


        public MainWindow()
        {
            InitializeComponent();
            
            DropFilesHerePanel.FileList.Files = Model.ImportFiles;

            Model.PatientsChanged += (s) => { Invoke((Action)(() => RemovedPatientsPanel.AllRecordSets = s.Patients)); };

            Model.OutputFilesChanged += (ConsentToolModel m) =>
            {
                LoadedFilesPanel.FileList.Files.Clear();
                LoadedFilesPanel.FileList.Files.AddRange(m.OutputFiles);
            };


            Model.OutputFilesChanged += (m) => { 
                Invoke((Action)( 
                    () => CheckIfSaveButtonCanBeEnabled(m.OutputFiles)
                )); 
            } ;


            Model.OutputFilesChanged += (m) => { Invoke((Action)UpdateBlankFilesRemovedLabel); };

            Model.Progress += Progress_Updated;
            RemovedPatientsPanel.MainWindowReference = this;

            CheckIfAdministrator();
        }

        private void Progress_Updated(object sender, ProgressEventArgs e)
        {
            if (progress is null)
            {
                progress = new ProgressForm();
                AddLockingForm(progress);
            }
            
            if (progress.InvokeRequired)
            {
                Invoke((Action<ProgressEventArgs>)UpdateProgressForm, new object[] { e });
            } else
            {
                Invoke((Action)progress.Show);
                UpdateProgressForm(e);
            }

            
        }

        private void UpdateProgressForm(ProgressEventArgs e)
        {
            
            progress.MaximumValue = Enum.GetValues(typeof(ConsentToolProgress.Stages)).Length;
            progress.Value = Array.IndexOf(Enum.GetValues(typeof(ConsentToolProgress.Stages)), e.ProgressInfo.stage);
            progress.LoadingText = e.ProgressInfo.StageToString();

            if (e.ProgressInfo.stage == ConsentToolProgress.Stages.Finished)
            {
                progress.Hide();
                RemoveLockingForm(progress);
                progress = null;
            }
        }

        private void UpdateBlankFilesRemovedLabel()
        {
            IEnumerable<Specification.File> SpecificationFilesInInput = Model.ImportFiles.GroupBy
                <ImportFile, Specification.File, Specification.File>
                (file => file.SpecificationFile,    //group by specification file
                (SpecificationFile, ImportFileIEnumerable) => SpecificationFile //output specification file from IGrouping
                );

            IEnumerable<Specification.File> SpecificationFilesInOutput = Model.OutputFiles.Select(OutFile => OutFile.SpecificationFile);

            List<Specification.File> SpecificationFilesThatDidntMakeIt = SpecificationFilesInInput.Except(SpecificationFilesInOutput).ToList();

            SpecificationFilesThatDidntMakeIt.RemoveAll(element => element is null);

            List<ImportFile> InputFilesThatDidntMakeIt = Model.ImportFiles.Where(inputFile => SpecificationFilesThatDidntMakeIt.Contains(inputFile.SpecificationFile)).ToList();

            

            const string BlankFilesRemovedLabelName = "BlankFilesRemovedLabel";

            string LabelText = "";
            if (InputFilesThatDidntMakeIt.Count() != 0)  {
                LabelText = $"{InputFilesThatDidntMakeIt.Count()} files were excluded from the output: {Environment.NewLine}{string.Join(Environment.NewLine, InputFilesThatDidntMakeIt.Select(file => $"-{file.Name}").ToArray())}";
            }
            
            Label ExistingLabel = (Label)LoadedFilesPanel.FileList.BottomPanel.Controls.Cast<Control>().Where(ctrl => ctrl.Name == BlankFilesRemovedLabelName).FirstOrDefault();
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
               ExistingLabel.Invoke((Action)(() => ExistingLabel.Text = LabelText));
            }

        }

        /// <summary>
        /// Checks if the program is being run with administrative privileges as this can cause drag and drop to not work correctly
        /// </summary>
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

        private bool ConfirmNationalOptOutChoice()
		{
            const string NOOFileNameInSpec = ".dat";
            //enter if either true & false or false & true
            bool NOOChecked = Model.EnableNationalOptOut;
            bool NOOFilePresent = Model.ImportFiles.Where(importFile => {
                if (importFile.SpecificationFile == null)    //would like to use bool? but not supported in .NET Framework
				{
                    return false;
				} else
				{
                    return ((bool)importFile.SpecificationFile?.Name.Contains(NOOFileNameInSpec)) && importFile.IsValid.ErrorLevel == ValidState.Good;
                }
                
            }).Count() > 0;
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

        /// <summary>
        /// Solves Directory.Delete's fun hidden feature of race conditions by dele
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="timeoutInMilliseconds"></param>
        /// <returns></returns>
        private bool DeleteDirectorySync(string directory, bool recursive, int timeoutInMilliseconds = 5000)
        {
            if (!Directory.Exists(directory))
            {
                return true;
            }

            var watcher = new FileSystemWatcher
            {
                Path = Path.Combine(directory, ".."),
                NotifyFilter = NotifyFilters.DirectoryName,
                Filter = directory,
            };
            var task = Task.Run(() => watcher.WaitForChanged(WatcherChangeTypes.Deleted, timeoutInMilliseconds));

            // we must not start deleting before the watcher is running
            while (task.Status != TaskStatus.Running)
            {
                System.Threading.Thread.Sleep(100);
            }

            try
            {
                Directory.Delete(directory, recursive);
            }
            catch
            {
                return false;
            }

            return !task.Result.TimedOut;
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if (ConfirmNationalOptOutChoice() != true)
			{
                return;
			}
            CommonOpenFileDialog SelectDialog = new CommonOpenFileDialog();

            if (DropFilesHerePanel.MostCommonSourceDirectory != null)
                SelectDialog.InitialDirectory = DropFilesHerePanel.MostCommonSourceDirectory;
            
            SelectDialog.IsFolderPicker = true;
            if (SelectDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string OutputFolder = SelectDialog.FileName + $"\\Processed {DateTime.Now.ToString("dd_MM_yyyy")}\\";

                if (Directory.Exists(OutputFolder))
                {
                    DialogResult output = MessageBox.Show("Output folder already exists, do you want to overwrite it?", "Output folder already exists", MessageBoxButtons.YesNo);
                    if (output == DialogResult.Yes)
                    {
                        try
                        {
                            DeleteDirectorySync(OutputFolder, true);
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

                


                ProgressForm repackingProgressForm = new ProgressForm();

                this.AddLockingForm(repackingProgressForm);
                repackingProgressForm.LoadingText = "Repacking output files for TRE";
                repackingProgressForm.Show();

                await Task.Run(
                    () => {
                        var Outfiles = LoadedFilesPanel.FileList.Files;
                        repackingProgressForm.MaximumValue = Outfiles.Count;


                        List<(string Content, string SpecificationFileName)> RepackedFiles = Outfiles
                                                    .AsParallel()
                                                    .AsOrdered()
                                                    .WithProgressReporting(() => repackingProgressForm.Value++)
                                                    .Select(OutFile => (Content: OutFile.StringOutput(), SpecificationFileName: OutFile.SpecificationFile.Name))
                                                    .ToList();

                        DirectoryInfo OutputDirectory = Directory.CreateDirectory(OutputFolder);

                        foreach ((string Content, string SpecificationFileName) RepackedOutFile in RepackedFiles)
                        {
                            //Remove wildcard and append _Processed to filename
                            string FileName = RepackedOutFile.SpecificationFileName.Replace(@".*\", "").Replace(".csv", "_Processed.csv");
                            string OutPath = Path.Combine(OutputDirectory.FullName, FileName);
#if (!DEBUG)
 try
                    {
#endif

                            var DirectoryExistsBeforeUse = Directory.Exists(OutputFolder);


                            using (StreamWriter writer = new StreamWriter(OutPath))
                            {
                                writer.Write(RepackedOutFile.Content);
                            }
#if (!DEBUG)
}

                    catch (Exception ex)
					{
                        MessageBox.Show($"{ex.Message} \n this will not have been completed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        continue;
					}
#endif
                        }

                        

                    }
                );

                repackingProgressForm.Close();
                this.RemoveLockingForm(repackingProgressForm);

                if (Model.Errors.Count() != 0)
                {
                    ErrorsWindow ErrorsWindow = new ErrorsWindow(Model.Errors);
                    ErrorsWindow.ShowDialog();
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

        private void CheckIfSaveButtonCanBeEnabled(OutputFile[] OutputFiles)
        {
            if (OutputFiles.Length > 0)
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
            Invoke((Action)CheckIfPatientsPaneFilterCheckboxesCanBeEnabled);
        }
        private void CheckIfPatientsPaneFilterCheckboxesCanBeEnabled()
        {
            if (RemovedPatientsPanel.AllRecordSets != null)
            {
                DisplayKeptPatientsCheckbox.Enabled = true;
                DisplayRemovedPatientsCheckbox.Enabled = true;
                CopyToClipboardButton.Enabled = true;
                CopyStatusLabel.Enabled = true;
            }
            else
            {
                DisplayKeptPatientsCheckbox.Enabled = false;
                DisplayRemovedPatientsCheckbox.Enabled = false;
                CopyToClipboardButton.Enabled = false;
                CopyStatusLabel.Enabled = false;
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
            SetMiddlePaneFilterFromUI();
        }
        private void SetMiddlePaneFilterFromUI()
        {
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
            Model.EnableNationalOptOut = CheckOptOutFile.Checked;   //Set designer value as default value, currently this is true
            Model.EnableConsentCheck = CheckConsent.Checked;

            SetMiddlePaneFilterFromUI();
            CheckIfPatientsPaneFilterCheckboxesCanBeEnabled();

            Version AssemblyVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Text = Text + $" v{AssemblyVersion.Major}.{AssemblyVersion.Minor}{(AssemblyVersion.MinorRevision != 0 ? "." + AssemblyVersion.MinorRevision.ToString() : "")}";
        }


        private async void CheckOptOutFile_CheckedChanged(object sender, EventArgs e)
        {
            await Task.Run(() => Model.EnableNationalOptOut = CheckOptOutFile.Checked);
        }

		private void GetManualLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            System.Diagnostics.Process.Start("https://ibdregistry.org.uk/extract-filter-guide");
		}

		private async void CopyToClipboardButton_Click(object sender, EventArgs e)
		{
            List<RecordSet> RecordSetsToExport = RemovedPatientsPanel.FilteredRecords;

            if (RecordSetsToExport.Count == 0)
			{
                return;
			}


            ProgressForm copyToCliboardProgressForm = new ProgressForm();

            this.AddLockingForm(copyToCliboardProgressForm);
            copyToCliboardProgressForm.LoadingText = "Copying middle pane to clipboard";
            copyToCliboardProgressForm.Show();
            
            await Task.Run(
                () => {
                        var Outputs = new List<(string Header, Func<RecordSet, string> CellValueGenerator)>() {
                            ("NHS Number", (RecordSet rs) => rs.GetFieldValue(DataItemCodes.NHSNumber).First()),
                            ("Date of Birth", (RecordSet rs) => rs.GetFieldValue(DataItemCodes.DateOfBirth).First()),
                            ("Kept/Removed", (RecordSet rs) => rs.IsConsentValid.IsValid ? "Kept" : "Removed"),
                            ("Reason", (RecordSet rs) => rs.IsConsentValid.IsValidReason)
                        };



                        string[] FieldsToInclude = { DataItemCodes.NHSNumber, DataItemCodes.DateOfBirth };

                        const string ColumnDelimiter = "\t";
                        const string RowDelimiter = "\r\n";
                        copyToCliboardProgressForm.MaximumValue = RecordSetsToExport.Count;

                        //get the field values to include (NHS Number, Forename, etc...) from each RecordSet and remove the conflicting characters from each value
                        IEnumerable<IEnumerable<string>> TableToExport = RecordSetsToExport
                                                                            .Select(rs => Outputs.Select(rsProcessor => rsProcessor.CellValueGenerator(rs))
                                                                            .Select(fieldValue => RepackingOutputFile.RemoveConflictingChars(fieldValue, new string[] { ColumnDelimiter })));

                        List<string> LinesToExport = TableToExport
                                                            .AsParallel()
                                                            .AsOrdered()
                                                            .WithProgressReporting(() => copyToCliboardProgressForm.Value++)
                                                            .Select(columns => string.Join(ColumnDelimiter, columns))
                                                            .ToList();
                        string Headers = string.Join(ColumnDelimiter, Outputs.Select(x => x.Header));
                        LinesToExport.Insert(0, Headers);

                        string CombinedOutput = string.Join(RowDelimiter, LinesToExport);

                        this.Invoke((Action)(() => Clipboard.SetText(CombinedOutput)));
                }
            );

            copyToCliboardProgressForm.Close();
            this.RemoveLockingForm(copyToCliboardProgressForm);
            


            string DefaultText = "Copies contents of processing pane onto clipboard";
            const string CopyText = "Copied to clipboard!";

            CopyStatusLabel.Text = CopyText;
            System.Drawing.Font ItalicisedLabelFont = new System.Drawing.Font(DefaultFont, DefaultFont.Style | System.Drawing.FontStyle.Italic);
            CopyStatusLabel.Font = ItalicisedLabelFont;

            System.Timers.Timer TimeOutTimer = new System.Timers.Timer();
            TimeOutTimer.Interval = 5000;
            TimeOutTimer.Enabled = true;
            TimeOutTimer.Elapsed += (object SendingTimer, ElapsedEventArgs TimerEventArgs) => {

                CopyStatusLabel.Invoke((Action)(() =>
                {
                    CopyStatusLabel.Text = DefaultText;

                    CopyStatusLabel.Font = Label.DefaultFont;
                }));
                

                var CastSendingTimer = (System.Timers.Timer)SendingTimer;
                CastSendingTimer.Enabled = false;
            };
        }

        private async void CheckConsent_CheckedChanged(object sender, EventArgs e)
        {
            await Task.Run(() => Model.EnableConsentCheck = CheckConsent.Checked);
        }
    }

    public static class Extensions
	{
        public static ParallelQuery<T> WithProgressReporting<T>(this ParallelQuery<T> sequence, Action increment)
		{
            return sequence.Select(x =>
            {
                increment?.Invoke();
                return x;
            });
		}
	}
}
