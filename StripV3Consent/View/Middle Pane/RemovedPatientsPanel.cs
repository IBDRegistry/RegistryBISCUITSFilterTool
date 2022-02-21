using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    class RemovedPatientsPanel: Panel
    {
        private ObservableCollection<RecordSet> allRecordSets;
        public ObservableCollection<RecordSet> AllRecordSets
        {
            get => allRecordSets;
            set
            {
                allRecordSets = value;
                AllRecordSetsChanged?.Invoke(this, new EventArgs());
                SetDisplayRecords();
            }
        }
        public event EventHandler AllRecordSetsChanged;

        private Func<RecordSet, bool> specifier = RecordSet => RecordSet.IsConsentValid == false;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Bindable(false)]
        [Browsable(false)]
        public Predicate<RecordSet> Specifier
        {
            get => new Predicate<RecordSet>(specifier);
            set
            {
                specifier = new Func<RecordSet, bool>(value);
                SetDisplayRecords();
            }
        }

        private void SetDisplayRecords()
        {
            if (!(AllRecordSets is null | Specifier is null)) { 
                DisplayRecords = new BindingList<RecordSet>(AllRecordSets.Where(specifier).ToList<RecordSet>());
            }
        }
        private BindingList<RecordSet> displayRecords;

        public BindingList<RecordSet> DisplayRecords
        {
            get => displayRecords;
            set
            {
                displayRecords = value;
                if (displayRecords is null) { return; }
                displayRecords.ListChanged += RemovedRecords_ListChanged;
                RemovedRecords_Rebuild();
            }
        }

        public RemovedPatientsPanel()
        {
            AutoScroll = true;

        }

        private void RemovedRecords_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    Controls.Add(new RemovedPatient { Patient = DisplayRecords[e.NewIndex] });
                    break;
                case ListChangedType.Reset:
                    RemovedRecords_Rebuild();
                    break;
            }
        }


        private void RemovedRecords_Rebuild()
        {
            Controls.Clear();
            RemovedPatient[] NewRemovedPatientPanels = DisplayRecords.Select(RS => new RemovedPatient() { 
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
