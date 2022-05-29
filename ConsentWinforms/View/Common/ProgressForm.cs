using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    public partial class ProgressForm : Form
    {
        public int MaximumValue
        {
            set
            {
                Invoke((MethodInvoker)(() => LoadingBar.Maximum = value));
            }
        }

        public int Value
        {
            set
            {
                Invoke((MethodInvoker)(() => LoadingBar.Value = value));
            }
        }
        public string LoadingText
        {
            set
            {
                if (InvokeRequired)
                {
                    Invoke((Action)(() => ProgressLabel.Text = value));
                } else
                {
                    ProgressLabel.Text = value;
                }
                
            }
        }

        public ProgressForm()
        {
            InitializeComponent();
        }
    }
}
