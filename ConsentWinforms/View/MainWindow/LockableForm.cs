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

        public LockableForm()
        {
            SetUpMessageFilter();
        }

        public void AddLockingForm(Form LockingForm)
        {
            LockingForms.Add(LockingForm);
        }
        public void RemoveLockingForm(Form LockingFormToRemove)
        {
            LockingForms.Remove(LockingFormToRemove);
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
            while (p!= null)
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
                if (FilteredWindowMessages.Contains(m.Msg))
                {
                    if (LockingForms.Count() > 0)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        LockingForms.Last<Form>().BringToFront();
                        return true;
                    }
                }
            }
            return false;
        }


    }
}
