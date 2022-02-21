
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
			this.autoResizingLabel2 = new StripV3Consent.View.AutoResizingLabel();
			this.LoadedFilesPanel = new StripV3Consent.View.OutputFilesPanel();
			this.RemovedPatientsPanel = new StripV3Consent.View.RemovedPatientsPanel();
			this.SaveButton = new System.Windows.Forms.Button();
			this.ImportPanelLabel = new System.Windows.Forms.Label();
			this.ProcessingPaneLabel = new System.Windows.Forms.Label();
			this.OutputPaneLabel = new System.Windows.Forms.Label();
			this.ProcessingControlPanel = new System.Windows.Forms.Panel();
			this.DisplayRemovedPatientsCheckbox = new System.Windows.Forms.CheckBox();
			this.DisplayKeptPatientsCheckbox = new System.Windows.Forms.CheckBox();
			this.autoResizingLabel1 = new StripV3Consent.View.AutoResizingLabel();
			this.TableLayoutPanel.SuspendLayout();
			this.DropFilesHerePanel.SuspendLayout();
			this.ProcessingControlPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// TableLayoutPanel
			// 
			this.TableLayoutPanel.ColumnCount = 3;
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 260F));
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
			this.TableLayoutPanel.Controls.Add(this.DropFilesHerePanel, 0, 1);
			this.TableLayoutPanel.Controls.Add(this.LoadedFilesPanel, 2, 1);
			this.TableLayoutPanel.Controls.Add(this.RemovedPatientsPanel, 1, 1);
			this.TableLayoutPanel.Controls.Add(this.SaveButton, 2, 2);
			this.TableLayoutPanel.Controls.Add(this.ImportPanelLabel, 0, 0);
			this.TableLayoutPanel.Controls.Add(this.ProcessingPaneLabel, 1, 0);
			this.TableLayoutPanel.Controls.Add(this.OutputPaneLabel, 2, 0);
			this.TableLayoutPanel.Controls.Add(this.ProcessingControlPanel, 1, 2);
			this.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.TableLayoutPanel.Name = "TableLayoutPanel";
			this.TableLayoutPanel.RowCount = 3;
			this.TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
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
			this.DropFilesHerePanel.Location = new System.Drawing.Point(3, 23);
			this.DropFilesHerePanel.Name = "DropFilesHerePanel";
			this.DropFilesHerePanel.Size = new System.Drawing.Size(254, 338);
			this.DropFilesHerePanel.TabIndex = 0;
			// 
			// autoResizingLabel2
			// 
			this.autoResizingLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.autoResizingLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.28F);
			this.autoResizingLabel2.Location = new System.Drawing.Point(0, 0);
			this.autoResizingLabel2.Name = "autoResizingLabel2";
			this.autoResizingLabel2.Size = new System.Drawing.Size(254, 338);
			this.autoResizingLabel2.TabIndex = 0;
			this.autoResizingLabel2.Text = "Drop files here";
			this.autoResizingLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LoadedFilesPanel
			// 
			this.LoadedFilesPanel.BackColor = System.Drawing.SystemColors.ControlLight;
			this.LoadedFilesPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LoadedFilesPanel.Location = new System.Drawing.Point(523, 23);
			this.LoadedFilesPanel.Name = "LoadedFilesPanel";
			this.LoadedFilesPanel.Size = new System.Drawing.Size(274, 338);
			this.LoadedFilesPanel.TabIndex = 1;
			// 
			// RemovedPatientsPanel
			// 
			this.RemovedPatientsPanel.AllRecordSets = null;
			this.RemovedPatientsPanel.AutoScroll = true;
			this.RemovedPatientsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.RemovedPatientsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RemovedPatientsPanel.Location = new System.Drawing.Point(263, 23);
			this.RemovedPatientsPanel.Name = "RemovedPatientsPanel";
			this.RemovedPatientsPanel.Size = new System.Drawing.Size(254, 338);
			this.RemovedPatientsPanel.TabIndex = 3;
			this.RemovedPatientsPanel.AllRecordSetsChanged += new System.EventHandler(this.RemovedPatientsPanel_AllRecordSetsChanged);
			// 
			// SaveButton
			// 
			this.SaveButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SaveButton.Enabled = false;
			this.SaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SaveButton.Location = new System.Drawing.Point(523, 367);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(274, 80);
			this.SaveButton.TabIndex = 4;
			this.SaveButton.Text = "Save output";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// ImportPanelLabel
			// 
			this.ImportPanelLabel.AutoSize = true;
			this.ImportPanelLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ImportPanelLabel.Location = new System.Drawing.Point(3, 0);
			this.ImportPanelLabel.Name = "ImportPanelLabel";
			this.ImportPanelLabel.Size = new System.Drawing.Size(254, 20);
			this.ImportPanelLabel.TabIndex = 5;
			this.ImportPanelLabel.Text = "File import pane";
			this.ImportPanelLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ProcessingPaneLabel
			// 
			this.ProcessingPaneLabel.AutoSize = true;
			this.ProcessingPaneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProcessingPaneLabel.Location = new System.Drawing.Point(263, 0);
			this.ProcessingPaneLabel.Name = "ProcessingPaneLabel";
			this.ProcessingPaneLabel.Size = new System.Drawing.Size(254, 20);
			this.ProcessingPaneLabel.TabIndex = 6;
			this.ProcessingPaneLabel.Text = "Processing pane";
			this.ProcessingPaneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// OutputPaneLabel
			// 
			this.OutputPaneLabel.AutoSize = true;
			this.OutputPaneLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OutputPaneLabel.Location = new System.Drawing.Point(523, 0);
			this.OutputPaneLabel.Name = "OutputPaneLabel";
			this.OutputPaneLabel.Size = new System.Drawing.Size(274, 20);
			this.OutputPaneLabel.TabIndex = 7;
			this.OutputPaneLabel.Text = "Output pane";
			this.OutputPaneLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ProcessingControlPanel
			// 
			this.ProcessingControlPanel.Controls.Add(this.DisplayRemovedPatientsCheckbox);
			this.ProcessingControlPanel.Controls.Add(this.DisplayKeptPatientsCheckbox);
			this.ProcessingControlPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProcessingControlPanel.Location = new System.Drawing.Point(263, 367);
			this.ProcessingControlPanel.Name = "ProcessingControlPanel";
			this.ProcessingControlPanel.Size = new System.Drawing.Size(254, 80);
			this.ProcessingControlPanel.TabIndex = 8;
			// 
			// DisplayRemovedPatientsCheckbox
			// 
			this.DisplayRemovedPatientsCheckbox.AutoSize = true;
			this.DisplayRemovedPatientsCheckbox.Checked = true;
			this.DisplayRemovedPatientsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.DisplayRemovedPatientsCheckbox.Enabled = false;
			this.DisplayRemovedPatientsCheckbox.Location = new System.Drawing.Point(4, 28);
			this.DisplayRemovedPatientsCheckbox.Name = "DisplayRemovedPatientsCheckbox";
			this.DisplayRemovedPatientsCheckbox.Size = new System.Drawing.Size(137, 17);
			this.DisplayRemovedPatientsCheckbox.TabIndex = 1;
			this.DisplayRemovedPatientsCheckbox.Text = "Show removed patients";
			this.DisplayRemovedPatientsCheckbox.UseVisualStyleBackColor = true;
			this.DisplayRemovedPatientsCheckbox.CheckedChanged += new System.EventHandler(this.DisplayCheckboxesChanged);
			// 
			// DisplayKeptPatientsCheckbox
			// 
			this.DisplayKeptPatientsCheckbox.AutoSize = true;
			this.DisplayKeptPatientsCheckbox.Enabled = false;
			this.DisplayKeptPatientsCheckbox.Location = new System.Drawing.Point(4, 4);
			this.DisplayKeptPatientsCheckbox.Name = "DisplayKeptPatientsCheckbox";
			this.DisplayKeptPatientsCheckbox.Size = new System.Drawing.Size(117, 17);
			this.DisplayKeptPatientsCheckbox.TabIndex = 0;
			this.DisplayKeptPatientsCheckbox.Text = "Show kept patients";
			this.DisplayKeptPatientsCheckbox.UseVisualStyleBackColor = true;
			this.DisplayKeptPatientsCheckbox.CheckedChanged += new System.EventHandler(this.DisplayCheckboxesChanged);
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
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainWindow";
			this.Text = "Strip V3 Consent";
			this.TableLayoutPanel.ResumeLayout(false);
			this.TableLayoutPanel.PerformLayout();
			this.DropFilesHerePanel.ResumeLayout(false);
			this.ProcessingControlPanel.ResumeLayout(false);
			this.ProcessingControlPanel.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TableLayoutPanel;
        private View.DropFilesPanel DropFilesHerePanel;
        private View.OutputFilesPanel LoadedFilesPanel;
        private View.AutoResizingLabel autoResizingLabel1;
        private View.RemovedPatientsPanel RemovedPatientsPanel;
        private View.AutoResizingLabel autoResizingLabel2;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label ImportPanelLabel;
        private System.Windows.Forms.Label ProcessingPaneLabel;
        private System.Windows.Forms.Label OutputPaneLabel;
        private System.Windows.Forms.Panel ProcessingControlPanel;
        private System.Windows.Forms.CheckBox DisplayRemovedPatientsCheckbox;
        private System.Windows.Forms.CheckBox DisplayKeptPatientsCheckbox;
    }
}

