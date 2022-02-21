using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    class RemovedPatientsPanel: Panel
    {
        private ObservableRangeCollection<RecordSet> allRecordSets;
        public ObservableRangeCollection<RecordSet> AllRecordSets
        {
            get => allRecordSets;
            set
            {
                allRecordSets = value;
                AllRecordSetsChanged?.Invoke(this, new EventArgs());
                if (value != null)
				    value.CollectionChanged += AllRecordSets_CollectionChanged;
            }
        }

		private void AllRecordSets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
            AllRecordSetsChanged?.Invoke(this, new EventArgs());
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    RemovedRecords_Redraw();
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RemovedRecords_Redraw();
                    break;
                case NotifyCollectionChangedAction.Remove:
                    throw new NotImplementedException();
            }
        }

        /// <summary>
        /// This event notifies when either the AllRecordSetsCollection is replaced or changed, to change the checkboxes at the bottom of the list
        /// </summary>
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
                RemovedRecords_Redraw();
            }
        }

  //      private void SetDisplayRecords()
  //      {
		//	//if (!(AllRecordSets is null | Specifier is null))
		//	//{
		//	//	DisplayRecords = new BindingList<RecordSet>(AllRecordSets.Where(specifier).ToList<RecordSet>());
		//	//}
		//}
        //private BindingList<RecordSet> displayRecords;

        //public BindingList<RecordSet> DisplayRecords
        //{
        //    get => displayRecords;
        //    set
        //    {
        //        displayRecords = value;
        //        if (displayRecords is null) { return; }
        //        RemovedRecords_Redraw();
        //    }
        //}
        
        public RemovedPatientsPanel()
        {
            AutoScroll = true;

        }


        /// <summary>
        /// Redraws the panel with all the records filtered by Specifi
        /// </summary>
        private void RemovedRecords_Redraw()
        {
            IEnumerable<RecordSet> DisplayRecords = AllRecordSets.Where(specifier);


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

        //private void AddItem(RecordSet Patient)
        //{

        //    RemovedPatient NewEntry = new RemovedPatient()
        //    {
        //        Patient = Patient,
        //        Dock = DockStyle.Top,
        //        Margin = new Padding()
        //        {
        //            Left = 5,
        //            Top = 10,
        //            Bottom = 10,
        //            Right = 5
        //        }
        //    };
        //    Controls.Add(NewEntry);
        //}

    }
}
