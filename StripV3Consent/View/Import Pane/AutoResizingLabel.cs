using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    class AutoResizingLabel: Label
    {
        [
        Category("Appearance"),
        Description("The size of the resizing text"),
        DefaultValue(0.6F)
        ]
        public float Ratio {
            get => mRatio;
            set { mRatio = value;
                Invalidate();            
            }
        }
        private float mRatio = 0.6F;

        public AutoResizingLabel()
        {
            TextAlign = ContentAlignment.MiddleCenter;
            AutoSize = false;
            Dock = DockStyle.Fill;

            Paint += ResizeOnPaint;

        }

        private void ResizeOnPaint(object sender, PaintEventArgs e)
        {

            float ratio = e.ClipRectangle.Height / 100.0F;
            if ((ratio > 0.1) && (ratio != mRatio))
            {
                Font = new Font(Font.FontFamily, 10.0F * ratio * mRatio, Font.Style);
            }
        }
    }
}
