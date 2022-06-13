using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    public class LockableForm : Form, IMessageFilter
    {
        private List<Form> LockingForms = new List<Form>();

        private List<Control> ControlsWithDragDropEnabled = new List<Control>();
        public LockableForm()
        {
            SetUpMessageFilter();
        }

        public void AddLockingForm(Form LockingForm)
        {
            LockingForms.Add(LockingForm);

            IEnumerable<Control> AllControlsOnLockedForm = GetAllChildControls(this);
            ControlsWithDragDropEnabled.AddRange(AllControlsOnLockedForm.Where(c => c.AllowDrop));

            foreach(Control c in this.ControlsWithDragDropEnabled)
            {
                c.Invoke((Action)(() => c.AllowDrop = false));
            }



        }
        
        private List<Control> GetAllChildControls(Control container, List<Control> AllControlsList = null)
        {
            if (AllControlsList is null)
                AllControlsList = new List<Control>();

            foreach (Control c in container.Controls)
            {
                GetAllChildControls(c, AllControlsList);
                AllControlsList.Add(c);
            }

            return AllControlsList;
        }
        public void RemoveLockingForm(Form LockingFormToRemove)
        {
            LockingForms.Remove(LockingFormToRemove);

            foreach (Control c in this.ControlsWithDragDropEnabled)
            {
                c.AllowDrop = true;
            }
            ControlsWithDragDropEnabled.Clear();
        }

        protected void SetUpMessageFilter()
        {
            Application.AddMessageFilter(this);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing) Application.RemoveMessageFilter(this);
            base.Dispose(disposing);
        }

        private bool HasParent(Control child, Control parent)
        {
            if (child == null) return false;
            Control p = child.Parent;
            while (p != null)
            {
                if (p == parent) return true;
                p = parent;
            }
            return false;
        }

        private readonly int[] FilteredWindowMessages = new int[]
        {
            0x201,  //WM_LBUTTONDOWN
            0x203,  //WM_LBUTTONDBLCLK
            0x202   //WM_LBUTTONUP
        };

        public bool PreFilterMessage(ref Message m)
        {
            Control c = Control.FromHandle(m.HWnd);
            if (HasParent(c, this) | m.HWnd == this.Handle)
            {
                //Block any click action
                if (FilteredWindowMessages.Contains(m.Msg))
                {
                    if (LockingForms.Count() > 0)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        LockingForms.Last<Form>().BringToFront();
                        return true;
                    }
                }

                if (c.AllowDrop)
                {

                }
            }
            return false;
        }


    }
}
