
namespace ConsentWinforms.View.MainWindow
{
    partial class ErrorsWindow
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
            this.components = new System.ComponentModel.Container();
            this.backingObjectBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.backingObjectBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.backingObjectBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.backingObjectBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // backingObjectBindingSource
            // 
            this.backingObjectBindingSource.DataSource = typeof(ConsentWinforms.View.MainWindow.BackingObject);
            // 
            // backingObjectBindingSource1
            // 
            this.backingObjectBindingSource1.DataSource = typeof(ConsentWinforms.View.MainWindow.BackingObject);
            // 
            // ErrorsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "ErrorsWindow";
            this.Text = "Errors";
            ((System.ComponentModel.ISupportInitialize)(this.backingObjectBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.backingObjectBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource backingObjectBindingSource;
        private System.Windows.Forms.BindingSource backingObjectBindingSource1;
    }
}