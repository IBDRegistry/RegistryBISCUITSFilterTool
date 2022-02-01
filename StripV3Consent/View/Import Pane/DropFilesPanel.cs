using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using StripV3Consent.Model;

namespace StripV3Consent.View
{
	class DropFilesPanel : Panel
	{
		public FileList<ImportFileItem, ImportFile> FileList = new FileList<ImportFileItem, ImportFile>();

		public DropFilesPanel()
		{
			this.AllowDrop = true;
			this.DragEnter += DropFiles_DragEnter;
			this.DragDrop += DropFiles_DragDrop;
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
			}
		}

		private void DropFiles_DragDrop(object sender, DragEventArgs e)
		{
			string[] InputPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
			string[] FilePaths = InputPaths.Where(path => !(File.GetAttributes(path) == FileAttributes.Directory | File.GetAttributes(path) == FileAttributes.Device)).ToArray(); //Prevent doctors from trying to drag and drop folders, devices and all sorts of nonsense
			this.Controls.Clear();
			this.Controls.Add(FileList);
			FileList.AddRange(FilePaths.Select<string, ImportFile>(Path => {
				string FileContents = null;
				using (StreamReader reader = new StreamReader(Path))
				{
					FileContents = reader.ReadToEnd();
				}
				return new ImportFile(System.IO.Path.GetFileName(Path), FileContents);
			}).ToArray());

			SourceFilePaths.AddRange(FilePaths);
		}

		private List<string> SourceFilePaths = new List<string>();

		public string MostCommonSourceDirectory { get {
				IEnumerable<string> SourceFilePathsRemaining = SourceFilePaths	//Some of the source paths may have been removed, only keep the ones that are still in the File List
																		.Where(SourcePath => FileList.Files
																										.Where(ImportFile => ImportFile.IsValid.IsValid == (ValidState.Good | ValidState.Warning))
																										.Select(ImportFile => ImportFile.Name)
																										.Contains(System.IO.Path.GetFileName(SourcePath)));


				return SourceFilePathsRemaining
									.GroupBy(Path => Path)
									.OrderBy(t => t.Count())
									.First().Key;
			} }


	}
}
