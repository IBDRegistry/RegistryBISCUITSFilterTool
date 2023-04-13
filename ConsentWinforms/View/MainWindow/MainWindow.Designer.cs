
namespace StripV3Consent.View
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DropFilesHerePanel = new StripV3Consent.View.DropFilesPanel();
            this.LoadedFilesPanel = new StripV3Consent.View.OutputFilesPanel();
            this.RemovedPatientsPanel = new StripV3Consent.View.RemovedPatientsPanel();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ImportPanelLabel = new System.Windows.Forms.Label();
            this.ProcessingPaneLabel = new System.Windows.Forms.Label();
            this.OutputPaneLabel = new System.Windows.Forms.Label();
            this.ProcessingControlPanel = new System.Windows.Forms.Panel();
            this.CopyStatusLabel = new System.Windows.Forms.Label();
            this.CopyToClipboardButton = new System.Windows.Forms.Button();
            this.DisplayRemovedPatientsCheckbox = new System.Windows.Forms.CheckBox();
            this.DisplayKeptPatientsCheckbox = new System.Windows.Forms.CheckBox();
            this.LeftControlPanel = new System.Windows.Forms.Panel();
            this.GetManualLabel = new System.Windows.Forms.LinkLabel();
            this.CheckOptOutMessage = new System.Windows.Forms.Label();
            this.CheckOptOutFile = new System.Windows.Forms.CheckBox();
            this.autoResizingLabel1 = new StripV3Consent.View.AutoResizingLabel();
            this.TableLayoutPanel.SuspendLayout();
            this.ProcessingControlPanel.SuspendLayout();
            this.LeftControlPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TableLayoutPanel
            // 
            this.TableLayoutPanel.ColumnCount = 3;
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 387F));
            this.TableLayoutPanel.Controls.Add(this.DropFilesHerePanel, 0, 1);
            this.TableLayoutPanel.Controls.Add(this.LoadedFilesPanel, 2, 1);
            this.TableLayoutPanel.Controls.Add(this.RemovedPatientsPanel, 1, 1);
            this.TableLayoutPanel.Controls.Add(this.SaveButton, 2, 2);
            this.TableLayoutPanel.Controls.Add(this.ImportPanelLabel, 0, 0);
            this.TableLayoutPanel.Controls.Add(this.ProcessingPaneLabel, 1, 0);
            this.TableLayoutPanel.Controls.Add(this.OutputPaneLabel, 2, 0);
            this.TableLayoutPanel.Controls.Add(this.ProcessingControlPanel, 1, 2);
            this.TableLayoutPanel.Controls.Add(this.LeftControlPanel, 0, 2);
            this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel.Margin = new System.Windows.Forms.Padding(4);
            this.TableLayoutPanel.Name = "TableLayoutPanel";
            this.TableLayoutPanel.RowCount = 3;
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel.Size = new System.Drawing.Size(1220, 688);
            this.TableLayoutPanel.TabIndex = 0;
            // 
            // DropFilesHerePanel
            // 
            this.DropFilesHerePanel.AllowDrop = true;
            this.DropFilesHerePanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.DropFilesHerePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DropFilesHerePanel.Location = new System.Drawing.Point(4, 29);
            this.DropFilesHerePanel.Margin = new System.Windows.Forms.Padding(4);
            this.DropFilesHerePanel.Name = "DropFilesHerePanel";
            this.DropFilesHerePanel.Size = new System.Drawing.Size(352, 522);
            this.DropFilesHerePanel.TabIndex = 0;
            // 
            // LoadedFilesPanel
            // 
            this.LoadedFilesPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.LoadedFilesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadedFilesPanel.Location = new System.Drawing.Point(837, 29);
            this.LoadedFilesPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LoadedFilesPanel.Name = "LoadedFilesPanel";
            this.LoadedFilesPanel.Size = new System.Drawing.Size(379, 522);
            this.LoadedFilesPanel.TabIndex = 1;
            // 
            // RemovedPatientsPanel
            // 
            this.RemovedPatientsPanel.AutoScroll = true;
            this.RemovedPatientsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.RemovedPatientsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RemovedPatientsPanel.Location = new System.Drawing.Point(364, 29);
            this.RemovedPatientsPanel.Margin = new System.Windows.Forms.Padding(4);
            this.RemovedPatientsPanel.Name = "RemovedPatientsPanel";
            this.RemovedPatientsPanel.Size = new System.Drawing.Size(465, 522);
            this.RemovedPatientsPanel.TabIndex = 3;
            this.RemovedPatientsPanel.AllRecordSetsChanged += new System.EventHandler(this.RemovedPatientsPanel_AllRecordSetsChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SaveButton.Enabled = false;
            this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.Location = new System.Drawing.Point(837, 559);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(379, 125);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save output";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ImportPanelLabel
            // 
            this.ImportPanelLabel.AutoSize = true;
            this.ImportPanelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImportPanelLabel.Location = new System.Drawing.Point(4, 0);
            this.ImportPanelLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ImportPanelLabel.Name = "ImportPanelLabel";
            this.ImportPanelLabel.Size = new System.Drawing.Size(352, 25);
            this.ImportPanelLabel.TabIndex = 5;
            this.ImportPanelLabel.Text = "File import pane";
            this.ImportPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProcessingPaneLabel
            // 
            this.ProcessingPaneLabel.AutoSize = true;
            this.ProcessingPaneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProcessingPaneLabel.Location = new System.Drawing.Point(364, 0);
            this.ProcessingPaneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ProcessingPaneLabel.Name = "ProcessingPaneLabel";
            this.ProcessingPaneLabel.Size = new System.Drawing.Size(465, 25);
            this.ProcessingPaneLabel.TabIndex = 6;
            this.ProcessingPaneLabel.Text = "Processing pane";
            this.ProcessingPaneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OutputPaneLabel
            // 
            this.OutputPaneLabel.AutoSize = true;
            this.OutputPaneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputPaneLabel.Location = new System.Drawing.Point(837, 0);
            this.OutputPaneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OutputPaneLabel.Name = "OutputPaneLabel";
            this.OutputPaneLabel.Size = new System.Drawing.Size(379, 25);
            this.OutputPaneLabel.TabIndex = 7;
            this.OutputPaneLabel.Text = "Output pane";
            this.OutputPaneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ProcessingControlPanel
            // 
            this.ProcessingControlPanel.Controls.Add(this.CopyStatusLabel);
            this.ProcessingControlPanel.Controls.Add(this.CopyToClipboardButton);
            this.ProcessingControlPanel.Controls.Add(this.DisplayRemovedPatientsCheckbox);
            this.ProcessingControlPanel.Controls.Add(this.DisplayKeptPatientsCheckbox);
            this.ProcessingControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProcessingControlPanel.Location = new System.Drawing.Point(364, 559);
            this.ProcessingControlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ProcessingControlPanel.Name = "ProcessingControlPanel";
            this.ProcessingControlPanel.Size = new System.Drawing.Size(465, 125);
            this.ProcessingControlPanel.TabIndex = 8;
            // 
            // CopyStatusLabel
            // 
            this.CopyStatusLabel.AutoSize = true;
            this.CopyStatusLabel.Location = new System.Drawing.Point(75, 92);
            this.CopyStatusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CopyStatusLabel.Name = "CopyStatusLabel";
            this.CopyStatusLabel.Size = new System.Drawing.Size(328, 17);
            this.CopyStatusLabel.TabIndex = 3;
            this.CopyStatusLabel.Text = "Copies contents of processing pane onto clipboard";
            // 
            // CopyToClipboardButton
            // 
            this.CopyToClipboardButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CopyToClipboardButton.Location = new System.Drawing.Point(5, 86);
            this.CopyToClipboardButton.Margin = new System.Windows.Forms.Padding(4);
            this.CopyToClipboardButton.Name = "CopyToClipboardButton";
            this.CopyToClipboardButton.Size = new System.Drawing.Size(61, 28);
            this.CopyToClipboardButton.TabIndex = 2;
            this.CopyToClipboardButton.Text = "Copy";
            this.CopyToClipboardButton.UseVisualStyleBackColor = true;
            this.CopyToClipboardButton.Click += new System.EventHandler(this.CopyToClipboardButton_Click);
            // 
            // DisplayRemovedPatientsCheckbox
            // 
            this.DisplayRemovedPatientsCheckbox.AutoSize = true;
            this.DisplayRemovedPatientsCheckbox.Checked = true;
            this.DisplayRemovedPatientsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayRemovedPatientsCheckbox.Enabled = false;
            this.DisplayRemovedPatientsCheckbox.Location = new System.Drawing.Point(5, 34);
            this.DisplayRemovedPatientsCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.DisplayRemovedPatientsCheckbox.Name = "DisplayRemovedPatientsCheckbox";
            this.DisplayRemovedPatientsCheckbox.Size = new System.Drawing.Size(177, 21);
            this.DisplayRemovedPatientsCheckbox.TabIndex = 1;
            this.DisplayRemovedPatientsCheckbox.Text = "Show removed patients";
            this.DisplayRemovedPatientsCheckbox.UseVisualStyleBackColor = true;
            this.DisplayRemovedPatientsCheckbox.CheckedChanged += new System.EventHandler(this.DisplayCheckboxesChanged);
            // 
            // DisplayKeptPatientsCheckbox
            // 
            this.DisplayKeptPatientsCheckbox.AutoSize = true;
            this.DisplayKeptPatientsCheckbox.Enabled = false;
            this.DisplayKeptPatientsCheckbox.Location = new System.Drawing.Point(5, 5);
            this.DisplayKeptPatientsCheckbox.Margin = new System.Windows.Forms.Padding(4);
            this.DisplayKeptPatientsCheckbox.Name = "DisplayKeptPatientsCheckbox";
            this.DisplayKeptPatientsCheckbox.Size = new System.Drawing.Size(149, 21);
            this.DisplayKeptPatientsCheckbox.TabIndex = 0;
            this.DisplayKeptPatientsCheckbox.Text = "Show kept patients";
            this.DisplayKeptPatientsCheckbox.UseVisualStyleBackColor = true;
            this.DisplayKeptPatientsCheckbox.CheckedChanged += new System.EventHandler(this.DisplayCheckboxesChanged);
            // 
            // LeftControlPanel
            // 
            this.LeftControlPanel.Controls.Add(this.GetManualLabel);
            this.LeftControlPanel.Controls.Add(this.CheckOptOutMessage);
            this.LeftControlPanel.Controls.Add(this.CheckOptOutFile);
            this.LeftControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LeftControlPanel.Location = new System.Drawing.Point(4, 559);
            this.LeftControlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LeftControlPanel.Name = "LeftControlPanel";
            this.LeftControlPanel.Size = new System.Drawing.Size(352, 125);
            this.LeftControlPanel.TabIndex = 9;
            // 
            // GetManualLabel
            // 
            this.GetManualLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GetManualLabel.AutoSize = true;
            this.GetManualLabel.Location = new System.Drawing.Point(4, 101);
            this.GetManualLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GetManualLabel.Name = "GetManualLabel";
            this.GetManualLabel.Size = new System.Drawing.Size(205, 17);
            this.GetManualLabel.TabIndex = 4;
            this.GetManualLabel.TabStop = true;
            this.GetManualLabel.Text = "Read the Getting Started guide";
            this.GetManualLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GetManualLabel_LinkClicked);
            // 
            // CheckOptOutMessage
            // 
            this.CheckOptOutMessage.AutoEllipsis = true;
            this.CheckOptOutMessage.Enabled = false;
            this.CheckOptOutMessage.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CheckOptOutMessage.Location = new System.Drawing.Point(4, 30);
            this.CheckOptOutMessage.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CheckOptOutMessage.Name = "CheckOptOutMessage";
            this.CheckOptOutMessage.Size = new System.Drawing.Size(344, 55);
            this.CheckOptOutMessage.TabIndex = 3;
            this.CheckOptOutMessage.Text = "If this box is ticked, a .dat file of patients who have not chosen the NDOO shoul" +
    "d be loaded, otherwise all unconsented patients will be removed.";
            // 
            // CheckOptOutFile
            // 
            this.CheckOptOutFile.AutoSize = true;
            this.CheckOptOutFile.Enabled = false;
            this.CheckOptOutFile.Location = new System.Drawing.Point(4, 5);
            this.CheckOptOutFile.Margin = new System.Windows.Forms.Padding(4);
            this.CheckOptOutFile.Name = "CheckOptOutFile";
            this.CheckOptOutFile.Size = new System.Drawing.Size(317, 21);
            this.CheckOptOutFile.TabIndex = 2;
            this.CheckOptOutFile.Text = "National data opt-out compliance functionality";
            this.CheckOptOutFile.UseVisualStyleBackColor = true;
            this.CheckOptOutFile.CheckedChanged += new System.EventHandler(this.CheckOptOutFile_CheckedChanged);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 688);
            this.Controls.Add(this.TableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(1235, 724);
            this.Name = "MainWindow";
            this.Text = "Biscuits Processing Tool";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.TableLayoutPanel.ResumeLayout(false);
            this.TableLayoutPanel.PerformLayout();
            this.ProcessingControlPanel.ResumeLayout(false);
            this.ProcessingControlPanel.PerformLayout();
            this.LeftControlPanel.ResumeLayout(false);
            this.LeftControlPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
        private View.DropFilesPanel DropFilesHerePanel;
        private View.OutputFilesPanel LoadedFilesPanel;
        private View.AutoResizingLabel autoResizingLabel1;
        private View.RemovedPatientsPanel RemovedPatientsPanel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label ImportPanelLabel;
        private System.Windows.Forms.Label ProcessingPaneLabel;
        private System.Windows.Forms.Label OutputPaneLabel;
        private System.Windows.Forms.Panel ProcessingControlPanel;
        private System.Windows.Forms.CheckBox DisplayRemovedPatientsCheckbox;
        private System.Windows.Forms.CheckBox DisplayKeptPatientsCheckbox;
        private System.Windows.Forms.CheckBox CheckOptOutFile;
		private System.Windows.Forms.Label CheckOptOutMessage;
		private System.Windows.Forms.Panel LeftControlPanel;
		private System.Windows.Forms.LinkLabel GetManualLabel;
		private System.Windows.Forms.Button CopyToClipboardButton;
		private System.Windows.Forms.Label CopyStatusLabel;
	}
}

