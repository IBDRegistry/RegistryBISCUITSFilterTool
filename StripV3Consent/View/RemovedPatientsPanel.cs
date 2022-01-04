using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    class RemovedPatientsPanel: Panel
    {
        private BindingList<RecordSet> removedRecords;

        public BindingList<RecordSet> RemovedRecords
        {
            get => removedRecords;
            set
            {
                removedRecords = value;
                if (removedRecords is null) { return; }
                removedRecords.ListChanged += RemovedRecords_ListChanged;
                RemovedRecords_Rebuild();
            }
        }

        public RemovedPatientsPanel()
        {
            //FlowDirection = FlowDirection.TopDown;
            //WrapContents = false;
            AutoScroll = true;

        }

        private void RemovedRecords_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Controls.Add(new RemovedPatient { Patient = RemovedRecords[e.NewIndex] });
                    break;
                case ListChangedType.Reset:
                    RemovedRecords_Rebuild();
                    break;
            }
        }


        private void RemovedRecords_Rebuild()
        {
            Controls.Clear();
            RemovedPatient[] NewRemovedPatientPanels = RemovedRecords.Select(RS => new RemovedPatient() { 
                Patient = RS, 
                Dock = DockStyle.Top,
                Margin = new Padding()
                {
                    Left = 5,
                    Top = 10,
                    Bottom = 10,
                    Right = 5
                }
            }).ToArray();
            Controls.AddRange(NewRemovedPatientPanels);
        }

    }
}
