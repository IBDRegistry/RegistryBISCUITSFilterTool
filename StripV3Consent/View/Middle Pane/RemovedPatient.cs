using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MakarovDev.ExpandCollapsePanel;
using StripV3Consent.Model;

namespace StripV3Consent.View
{
    class RemovedPatient: ExpandCollapsePanel
    {
        private RecordSet patient;
        public RecordSet Patient
        {
            get => patient;
            set
            {
                patient = value;
                Draw();
                //int HeightOfAllControls = Controls.Cast<Control>().Select(control => control.Height).Aggregate()
            }
        }

        public RemovedPatient()
        {
            BorderStyle = BorderStyle.FixedSingle;
            UseAnimation = false;
            IsExpanded = false;
            UseAnimation = true;
            AutoSize = true;
            ForeColor = System.Drawing.Color.Black;

            //The header bar of the ExpandCollapsePanel is not docked so any controls docked will appear underneath it - not good
            //The perfect solution would be to have the top bar as DockStyle.Top and a filler panel as DockStyle.Fill
            //All requests to the Controls collection would be rerouted to the filler panel's Controls collection
            //However in the meantime a placeholder panel will dock underneath the header to prevent elements from appearing there

            Controls.Add(new Panel()
            {
                Height = Height,    //Same height as ExpandCollapsePanel in its collapesed state which is the header height
                Dock = DockStyle.Top,
            });


            

            
        }

        private void Draw()
        {
            Text = Patient.GetFieldValue(DataItemCodes.NHSNumber);

            Label RejectionReason = new Label()
            {
                Text = Patient.IsConsentValid.IsValidReason,
                Dock = DockStyle.Top
            };

            Controls.Add(RejectionReason);
            Controls.SetChildIndex(RejectionReason, 0);//See later comment about docking order

           Record[][] RecordsGroupedByOriginalFile = Patient.Records.GroupBy
                                                                            <Record, DataFile, Record[]>
                                                                            (r => r.OriginalFile,
                                                                            (OriginalFile, IEnumerableRecord) => IEnumerableRecord.ToArray()
                                                                            ).ToArray();

            foreach (Record[] records in RecordsGroupedByOriginalFile)
            {
                Label DataRecordsViewLabel = new Label()
                {
                    Text = records.First().OriginalFile.Name,
                    Dock = DockStyle.Top,
                    Margin = new Padding()
                    {
                        Top = 20
                    }
                };

                DataGridView DataRecordsView = new DataGridView()
                {
                    ScrollBars = ScrollBars.Horizontal,
                    RowHeadersVisible = false,
                    AllowUserToAddRows = false,
                    ReadOnly = true,
                    Dock = DockStyle.Top
                
                };
                foreach(string column in records.First().OriginalFile.SpecificationFile.Fields.Select(f => f.Name))
                {
                    DataRecordsView.Columns.Add(new DataGridViewColumn(new DataGridViewTextBoxCell()) 
                       {
                            HeaderText = column
                    });
                }

                foreach(Record r in records)
                {
                    DataRecordsView.Rows.Add(r.DataRecord);
                }
                
                

                DataRecordsView.Height = (DataRecordsView.Rows.GetRowsHeight(new DataGridViewElementStates()) / DataRecordsView.Rows.Count) * (DataRecordsView.Rows.Count + 2);

                this.Controls.Add(DataRecordsViewLabel);
                this.Controls.Add(DataRecordsView);
                Controls.SetChildIndex(DataRecordsView, 0);     //DockStyle order malarky seems to work through the list back to front
                Controls.SetChildIndex(DataRecordsViewLabel, 1);    //so in order to get the padding panel and info label at the top the DataGridView
                                                                    //elements have to be inserted at the beginning so that they are added last
            }
        }
    }
}
