using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace StripV3Consent.View
{
    public class PatientOutputFileItem: AbstractFileItem<RepackingOutputFile>
    {
        public PatientOutputFileItem(RepackingOutputFile file) : base(file)
        {
        }

        public override void DrawContents()
        {
            base.DrawContents();

            Panel KeptRemoveLabelsStackPanel = new FlowLayoutPanel()
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown
            };

            Label RecordsKeptLabel = new Label()
            {
                Text = $"{File.OutputRecords.Count()} records kept",
                Width = 120,
                ForeColor = Color.LimeGreen,
                AutoSize = true,
                Padding = new Padding()
                {
                    Bottom = 4
                }
            };
            KeptRemoveLabelsStackPanel.Controls.Add(RecordsKeptLabel);

            //int NumberOfRecordsRemoved;
            //if (File.AllRecordsOriginallyInFile == null || File.OutputRecords == null)
            //{
            //    NumberOfRecordsRemoved = 0;
            //} else
            //{
            //    NumberOfRecordsRemoved = File.AllRecordsOriginallyInFile.Except(File.OutputRecords).Count();
            //}
            Label RecordsRemovedLabel = new Label()
            {
                Text = $"{File.AllRecordsOriginallyInFile.Except(File.OutputRecords).Count()} records removed",
                ForeColor = Color.Red
                //Dock = DockStyle.Top
            };

            KeptRemoveLabelsStackPanel.Controls.Add(RecordsRemovedLabel);

            this.Controls.Add(KeptRemoveLabelsStackPanel);
        }
    }
}
