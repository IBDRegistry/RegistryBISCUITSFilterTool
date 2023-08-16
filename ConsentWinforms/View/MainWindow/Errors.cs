using StripConsentModel.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsentWinforms.View.MainWindow
{

    public partial class ErrorsWindow : Form
    {
        private BackingObject[] CollapseErrors(List<Error> errors) =>
            errors.GroupBy(x => (x.FilePath, x.ErrorDescription))
                .Select(x => new BackingObject(filePath: x.Key.FilePath, error: x.Key.ErrorDescription, x.Count()))
                .ToArray();

        public ErrorsWindow(List<Error> errors)
        {
            InitializeComponent();

            DataGridView ErrorsGrid = new DataGridView();
            ErrorsGrid.Dock = DockStyle.Fill;
            Controls.Add(ErrorsGrid);

            ErrorsGrid.AutoGenerateColumns = false;

            ErrorsGrid.Columns.AddRange(new DataGridViewTextBoxColumn[] {
                new DataGridViewTextBoxColumn()
                {
                    Name = "FilePath",
                    HeaderText = "File Path"
                },
                new DataGridViewTextBoxColumn()
                {
                    Name = "Error",
                    HeaderText = "Error"
                },
                new DataGridViewTextBoxColumn()
                {
                    Name = "Count",
                    HeaderText = "Count"
                }
            }
            );

            var backingObjects = CollapseErrors(errors);
            foreach (BackingObject backingObject in backingObjects)
            {
                ErrorsGrid.Rows.Add(backingObject.ToArray());
            }


        }
    }

    class BackingObject
    {
        public string FilePath;
        public string Error;
        public int Count;

        public BackingObject(string filePath, string error, int count)
        {
            FilePath = filePath;
            Error = error;
            Count = count;
        }

        public string[] ToArray()
        {
            return new string[] { FilePath, Error, Count.ToString() };
        }
    }
}
