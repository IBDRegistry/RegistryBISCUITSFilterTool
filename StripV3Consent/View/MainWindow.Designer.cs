
namespace StripV3Consent
{
    partial class MainWindow
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
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DropFilesHerePanel = new StripV3Consent.View.DropFilesPanel();
            this.autoResizingLabel2 = new StripV3Consent.View.AutoResizingLabel();
            this.LoadedFilesPanel = new StripV3Consent.View.OutputFilesPanel();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.RemovedPatientsPanel = new StripV3Consent.View.RemovedPatientsPanel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.autoResizingLabel1 = new StripV3Consent.View.AutoResizingLabel();
            this.TableLayoutPanel.SuspendLayout();
            this.DropFilesHerePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.ColumnCount = 3;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
            this.TableLayoutPanel.Controls.Add(this.DropFilesHerePanel, 0, 0);
            this.TableLayoutPanel.Controls.Add(this.LoadedFilesPanel, 2, 0);
            this.TableLayoutPanel.Controls.Add(this.ExecuteButton, 0, 1);
            this.TableLayoutPanel.Controls.Add(this.RemovedPatientsPanel, 1, 0);
            this.TableLayoutPanel.Controls.Add(this.SaveButton, 2, 1);
            this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowCount = 2;
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel.Size = new System.Drawing.Size(800, 450);
            this.TableLayoutPanel.TabIndex = 0;
            // 
            // DropFilesHerePanel
            // 
            this.DropFilesHerePanel.AllowDrop = true;
            this.DropFilesHerePanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.DropFilesHerePanel.Controls.Add(this.autoResizingLabel2);
            this.DropFilesHerePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DropFilesHerePanel.Location = new System.Drawing.Point(3, 3);
            this.DropFilesHerePanel.Name = "DropFilesHerePanel";
            this.DropFilesHerePanel.Size = new System.Drawing.Size(254, 354);
            this.DropFilesHerePanel.TabIndex = 0;
            // 
            // autoResizingLabel2
            // 
            this.autoResizingLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoResizingLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.24F);
            this.autoResizingLabel2.Location = new System.Drawing.Point(0, 0);
            this.autoResizingLabel2.Name = "autoResizingLabel2";
            this.autoResizingLabel2.Size = new System.Drawing.Size(254, 354);
            this.autoResizingLabel2.TabIndex = 0;
            this.autoResizingLabel2.Text = "Drop files here";
            this.autoResizingLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LoadedFilesPanel
            // 
            this.LoadedFilesPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LoadedFilesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadedFilesPanel.Location = new System.Drawing.Point(543, 3);
            this.LoadedFilesPanel.Name = "LoadedFilesPanel";
            this.LoadedFilesPanel.Size = new System.Drawing.Size(254, 354);
            this.LoadedFilesPanel.TabIndex = 1;
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExecuteButton.Enabled = false;
            this.ExecuteButton.Location = new System.Drawing.Point(3, 363);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(254, 84);
            this.ExecuteButton.TabIndex = 2;
            this.ExecuteButton.Text = "Go";
            this.ExecuteButton.UseVisualStyleBackColor = true;
            this.ExecuteButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // RemovedPatientsPanel
            // 
            this.RemovedPatientsPanel.AutoScroll = true;
            this.RemovedPatientsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RemovedPatientsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemovedPatientsPanel.Location = new System.Drawing.Point(263, 3);
            this.RemovedPatientsPanel.Name = "RemovedPatientsPanel";
            this.RemovedPatientsPanel.RemovedRecords = null;
            this.RemovedPatientsPanel.Size = new System.Drawing.Size(274, 354);
            this.RemovedPatientsPanel.TabIndex = 3;
            // 
            // SaveButton
            // 
            this.SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveButton.Enabled = false;
            this.SaveButton.Location = new System.Drawing.Point(543, 363);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(254, 84);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save output";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // autoResizingLabel1
            // 
            this.autoResizingLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoResizingLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 28.32F);
            this.autoResizingLabel1.Location = new System.Drawing.Point(0, 0);
            this.autoResizingLabel1.Name = "autoResizingLabel1";
            this.autoResizingLabel1.Ratio = 0.8F;
            this.autoResizingLabel1.Size = new System.Drawing.Size(254, 354);
            this.autoResizingLabel1.TabIndex = 0;
            this.autoResizingLabel1.Text = "Drop files here";
            this.autoResizingLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.TableLayoutPanel);
            this.Name = "MainWindow";
            this.Text = "Strip V3 Consent";
            this.TableLayoutPanel.ResumeLayout(false);
            this.DropFilesHerePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
        private View.DropFilesPanel DropFilesHerePanel;
        private View.OutputFilesPanel LoadedFilesPanel;
        private View.AutoResizingLabel autoResizingLabel1;
        private System.Windows.Forms.Button ExecuteButton;
        private View.RemovedPatientsPanel RemovedPatientsPanel;
        private View.AutoResizingLabel autoResizingLabel2;
        private System.Windows.Forms.Button SaveButton;
    }
}

