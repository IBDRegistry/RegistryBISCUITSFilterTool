using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StripV3Consent.View
{
    class ExecuteButton: Button
    {
        public ExecuteButton()
        {
            this.TextAlign = ContentAlignment.MiddleCenter;
            this.AutoSize = false;
            this.Dock = DockStyle.Fill;

            this.Text = "Drop files here";

            this.Paint += ResizeOnPaint;

            TextSizeRatio = 1.5F;
        }

        
        public float TextSizeRatio
        {
            get;set;
        }

        private void ResizeOnPaint(object sender, PaintEventArgs e)
        {
            

            float ratio = e.ClipRectangle.Height / 100.0F;
            if ((ratio > 0.1) && (ratio != TextSizeRatio))
            {
                TextSizeRatio = ratio;
                this.Font = new Font(Font.FontFamily, 10.0F * ratio, Font.Style);
            }
        }
    }
}
