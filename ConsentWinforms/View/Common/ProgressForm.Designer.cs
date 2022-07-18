
namespace StripV3Consent.View
{
    partial class ProgressForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.ProgressLabel = new System.Windows.Forms.Label();
            this.LoadingBar = new System.Windows.Forms.ProgressBar();
            this.ProgressLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ProgressLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgressLabel
            // 
            this.ProgressLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ProgressLabel.Location = new System.Drawing.Point(3, 45);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(422, 30);
            this.ProgressLabel.TabIndex = 0;
            this.ProgressLabel.Text = "Working";
            this.ProgressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadingBar
            // 
            this.LoadingBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.LoadingBar.Location = new System.Drawing.Point(20, 78);
            this.LoadingBar.Margin = new System.Windows.Forms.Padding(20, 3, 20, 3);
            this.LoadingBar.Name = "LoadingBar";
            this.LoadingBar.Size = new System.Drawing.Size(388, 23);
            this.LoadingBar.TabIndex = 1;
            // 
            // ProgressLayoutPanel
            // 
            this.ProgressLayoutPanel.ColumnCount = 1;
            this.ProgressLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ProgressLayoutPanel.Controls.Add(this.ProgressLabel, 0, 0);
            this.ProgressLayoutPanel.Controls.Add(this.LoadingBar, 0, 1);
            this.ProgressLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.ProgressLayoutPanel.Name = "ProgressLayoutPanel";
            this.ProgressLayoutPanel.RowCount = 2;
            this.ProgressLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.ProgressLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            this.ProgressLayoutPanel.Size = new System.Drawing.Size(428, 168);
            this.ProgressLayoutPanel.TabIndex = 2;
            // 
            // ProgressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 168);
            this.ControlBox = false;
            this.Controls.Add(this.ProgressLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ProgressForm";
            this.Text = "Working";
            this.ProgressLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ProgressLabel;
        private System.Windows.Forms.ProgressBar LoadingBar;
        private System.Windows.Forms.TableLayoutPanel ProgressLayoutPanel;
    }
}