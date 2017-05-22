namespace InfoFenix.Client.Views {
    partial class IndexDocumentDirectoryForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.indexingProgressBar = new System.Windows.Forms.ProgressBar();
            this.documentLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.fromToLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // indexingProgressBar
            // 
            this.indexingProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.indexingProgressBar.Location = new System.Drawing.Point(17, 42);
            this.indexingProgressBar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.indexingProgressBar.Name = "indexingProgressBar";
            this.indexingProgressBar.Size = new System.Drawing.Size(454, 30);
            this.indexingProgressBar.TabIndex = 0;
            // 
            // documentLabel
            // 
            this.documentLabel.AutoSize = true;
            this.documentLabel.Location = new System.Drawing.Point(13, 10);
            this.documentLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.documentLabel.Name = "documentLabel";
            this.documentLabel.Size = new System.Drawing.Size(45, 19);
            this.documentLabel.TabIndex = 1;
            this.documentLabel.Text = "###";
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(351, 88);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(120, 40);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "Parar";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // fromToLabel
            // 
            this.fromToLabel.AutoSize = true;
            this.fromToLabel.Location = new System.Drawing.Point(13, 88);
            this.fromToLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.fromToLabel.Name = "fromToLabel";
            this.fromToLabel.Size = new System.Drawing.Size(45, 19);
            this.fromToLabel.TabIndex = 3;
            this.fromToLabel.Text = "###";
            // 
            // IndexDocumentDirectoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(484, 146);
            this.Controls.Add(this.fromToLabel);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.documentLabel);
            this.Controls.Add(this.indexingProgressBar);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IndexDocumentDirectoryForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Indexando...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IndexDocumentDirectoryForm_FormClosing);
            this.Load += new System.EventHandler(this.IndexDocumentDirectoryForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar indexingProgressBar;
        private System.Windows.Forms.Label documentLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label fromToLabel;
    }
}