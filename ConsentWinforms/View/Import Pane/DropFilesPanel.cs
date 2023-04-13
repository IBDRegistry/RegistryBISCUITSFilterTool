using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using StripV3Consent.Model;
using System.Drawing;
using System.Threading.Tasks;

namespace StripV3Consent.View
{
	class DropFilesPanel : Panel
	{
		public FileList<ImportFileItem, ImportFile> FileList = new FileList<ImportFileItem, ImportFile>();

		public DropFilesPanel()
		{

			AllowDrop = true;
			DragEnter += DropFiles_DragEnter;
			DragDrop += DropFiles_DragDrop;

			CustomizeBottomPanel();

			Controls.Add(FileList);

        }

		private void CustomizeBottomPanel()
        {
			FileList.BottomPanel.Width = 240;

			Label BottomPanelLabel = new Label()
			{
				Text = "You can drop files here",
				AutoSize = true,
				//MaximumSize = new Size() { Width = 240, Height = 0 },
				Dock = DockStyle.Top,
				TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Microsoft Sans Serif", 14.5f)
			};
			
			Button BottomPanelFileDialogButton = new Button()
			{
				Text = "Alternatively, click here to browse for files",
				AutoSize = true,
				Dock = DockStyle.Top,
			};
            BottomPanelFileDialogButton.Click += BottomPanelFileDialogButton_Click;

			FileList.BottomPanel.Controls.Add(BottomPanelFileDialogButton);
			FileList.BottomPanel.Controls.Add(BottomPanelLabel);
		}

        private void BottomPanelFileDialogButton_Click(object sender, EventArgs e)
        {
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				//openFileDialog.InitialDirectory = "c:\\";
				openFileDialog.Filter = "csv and dat files |*.csv;*.dat|All files (*.*)|*.*";
				//openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.Multiselect = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					AddFiles(openFileDialog.FileNames);
				}
			}
		}

        private void DropFiles_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Move;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			};
		}

		/// <summary>
		/// Provides preliminary validation that a file can be safely loaded into the program
		/// </summary>
		/// <param name="FilePath"></param>
		/// <returns></returns>
		private bool IsFileValidForDropping(string FilePath)
        {
			if (!File.Exists(FilePath))
            {
				MessageBox.Show($"The file at {FilePath} doesn't exist and won't be imported into the application");
				return false;
            }

			FileAttributes Attributes = File.GetAttributes(FilePath);
			if (Attributes == FileAttributes.Directory | Attributes == FileAttributes.Device) //Prevent doctors from trying to drag and drop folders, devices and all sorts of nonsense
			{
				MessageBox.Show("Sorry but we don't support that type of file");
				return false;
            }

			const long MaxFileSize = 10000000; //10MB
			long FileSize = new FileInfo(FilePath).Length;
			if (FileSize > MaxFileSize)
            {
				MessageBox.Show($"That file is too large for this program to handle (Size was {FileSize} and Maximum Size is {MaxFileSize}");
				return false;
            }

			return true;
		}

		/// <summary>
		/// Takes any directories in Paths and replaces them with their contents
		/// </summary>
		/// <param name="Paths">A list of strings indicating paths indicating files or directories</param>
		/// <returns></returns>
		private void RecursivelyExpandFolders(List<string> Paths)
		{
			do
			{
				Func<string, bool> IsDirectory = path => File.GetAttributes(path).HasFlag(FileAttributes.Directory);

				string[] Directories = Paths.Where(IsDirectory).ToArray();	//Cast to string[] rather than keep as IEnumerable<string> otherwise it will be lost on the next line
				Paths.RemoveAll(new Predicate<string>(IsDirectory));

				IEnumerable<string> NewFiles = Directories.SelectMany(DirectoryPath =>
				{
					DirectoryInfo ExpandedDirectoryInfo = new DirectoryInfo(DirectoryPath);
					IEnumerable<string> FilesInDirectory = ExpandedDirectoryInfo.GetFiles().Select(FI => FI.FullName);
					IEnumerable<string> FoldersInDirectory = ExpandedDirectoryInfo.GetDirectories().Select(DI => DI.FullName);

					return FilesInDirectory.Union(FoldersInDirectory);
				});

				Paths.AddRange(NewFiles);
			} while (Paths.Select(path => File.GetAttributes(path)).Any(attr => attr == FileAttributes.Directory));
		}

		private async void DropFiles_DragDrop(object sender, DragEventArgs e)
		{
			List<string> InputPaths = ((string[])e.Data.GetData(DataFormats.FileDrop)).ToList<string>();
			RecursivelyExpandFolders(InputPaths);

			string[] FilePaths = InputPaths.Where(path => IsFileValidForDropping(path)).ToArray(); //Prevent doctors from trying to drag and drop folders, devices and all sorts of nonsense

			AddFiles(FilePaths);
		}


		private async void AddFiles(string[] FilePaths)
        {
			List<(string Path, string Content)> Contents = new List<(string Path, string Content)>();

			foreach (string path in FilePaths)
			{
				try
				{
					using (StreamReader reader = new StreamReader(path))
					{
						Contents.Add((Path: path, Content: await reader.ReadToEndAsync()));
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message + Environment.NewLine + Environment.NewLine + "This file will not have been imported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

					continue;
				}
			}

			ImportFile[] ImportFiles = Contents.Select(content => new ImportFile(
				FileName: Path.GetFileName(content.Path), 
				Contents: content.Content, 
				FileCreatedTimestamp: File.GetLastWriteTime(content.Path),
				FilePath: content.Path
				)).ToArray();
            await Task.Run(() => FileList.AddRange(ImportFiles));
		}


		public string MostCommonSourceDirectory {
			get
			{
				var ContainingDirectories = FileList.Files.Select(ImportFile => ImportFile.FilePath).Where(Path => Path != null).Select(Path => System.IO.Path.GetDirectoryName(Path));
				if (ContainingDirectories.Count() == 0) { return null; }
				return ContainingDirectories.GroupBy(Path => Path)
								   .OrderByDescending(t => t.Count())
								   .FirstOrDefault().Key;
			}
		}


	}
}
