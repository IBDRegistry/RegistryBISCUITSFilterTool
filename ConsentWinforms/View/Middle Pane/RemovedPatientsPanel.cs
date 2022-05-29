using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

        public MainWindow MainWindowReference;

		private void AllRecordSets_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
            AllRecordSetsChanged?.Invoke(this, new EventArgs());
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Invoke((Action)(() => RemovedRecords_Redraw()));
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Invoke((Action)(() => RemovedRecords_Redraw()));
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
        
        public RemovedPatientsPanel()
        {
            AutoScroll = true;

        }
        /// <summary>
        /// Redraws the panel with all the records filtered by Specifi
        /// </summary>
        private async void RemovedRecords_Redraw()
        {
            if (MainWindowReference is null)
                throw new NullReferenceException($"{nameof(MainWindowReference)} was null");

            ProgressForm filteringProgressForm = new ProgressForm();
            MainWindowReference.AddLockingForm(filteringProgressForm);
            filteringProgressForm.LoadingText = "Filtering records for middle pane";
            filteringProgressForm.Show();
            List<RecordSet> DisplayRecords = new List<RecordSet>();
            await Task.Run(
                () => {
                    for (int i = 0; i < AllRecordSets.Count; i++)
                    {
                        RecordSet r = AllRecordSets[i];
                        filteringProgressForm.Value = i;
                        filteringProgressForm.MaximumValue = AllRecordSets.Count;
                        if (specifier(r))
                        {
                            DisplayRecords.Add(r);
                        }
                    }
                }
            );

            filteringProgressForm.Close();
            MainWindowReference.RemoveLockingForm(filteringProgressForm);

            Controls.Clear();

            if (DisplayRecords.Count() > 100)
			{
                Controls.Add(new Label() { 
                    Text = "There are too many patients to display in this panel",
                    AutoSize = true,
                    Dock = DockStyle.Fill,
                    TextAlign = System.Drawing.ContentAlignment.TopCenter
                });
                return;
			}

            
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
