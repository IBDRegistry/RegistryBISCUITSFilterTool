using StripV3Consent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace StripV3Consent.View
{
    public class NonPatientOutputFileItem: OutputFileItem
    {
        public NonPatientOutputFileItem(OutputFile file) : base(file)
        {
        }

        public override void DrawContents()
        {
            base.DrawContents();

            Label NonPatientFileLabel = new WordWrappingLabel()
            {
                Text = "No patient records in file"
                
            };
            this.Controls.Add(NonPatientFileLabel);
        }

		private class WordWrappingLabel : Label
		{
			public WordWrappingLabel()
			{
                AutoSize = false;
			}

			protected override void OnPaint(PaintEventArgs e)
			{
				base.OnPaint(e);
                SizeF DimensionsOfText = e.Graphics.MeasureString(Text, Font);
                if (DimensionsOfText.Width > Width)
				{
                    Height = (int)(DimensionsOfText.Height * 2);
				}
			}
		}
	}
}
