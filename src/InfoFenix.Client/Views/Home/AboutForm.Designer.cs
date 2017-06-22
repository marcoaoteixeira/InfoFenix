namespace InfoFenix.Client.Views.Home {
    partial class AboutForm {
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
            this.applicationIconPictureBox = new System.Windows.Forms.PictureBox();
            this.applicationNameLabel = new System.Windows.Forms.Label();
            this.versionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.applicationIconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // applicationIconPictureBox
            // 
            this.applicationIconPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.applicationIconPictureBox.Image = global::InfoFenix.Client.Properties.Resources.info_fenix_84x95;
            this.applicationIconPictureBox.Location = new System.Drawing.Point(13, 13);
            this.applicationIconPictureBox.Name = "applicationIconPictureBox";
            this.applicationIconPictureBox.Size = new System.Drawing.Size(84, 95);
            this.applicationIconPictureBox.TabIndex = 0;
            this.applicationIconPictureBox.TabStop = false;
            this.applicationIconPictureBox.Click += new System.EventHandler(this.applicationIconPictureBox_Click);
            // 
            // applicationNameLabel
            // 
            this.applicationNameLabel.AutoSize = true;
            this.applicationNameLabel.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applicationNameLabel.Location = new System.Drawing.Point(103, 13);
            this.applicationNameLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.applicationNameLabel.Name = "applicationNameLabel";
            this.applicationNameLabel.Size = new System.Drawing.Size(153, 33);
            this.applicationNameLabel.TabIndex = 1;
            this.applicationNameLabel.Text = "Info Fênix";
            this.applicationNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(105, 56);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(45, 19);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "###";
            // 
            // AboutForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(264, 121);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.applicationNameLabel);
            this.Controls.Add(this.applicationIconPictureBox);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sobre";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.applicationIconPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox applicationIconPictureBox;
        private System.Windows.Forms.Label applicationNameLabel;
        private System.Windows.Forms.Label versionLabel;
    }
}