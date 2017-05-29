namespace InfoFenix.Client {
    partial class SplashScreenForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreenForm));
            this.splashScreenPanel = new System.Windows.Forms.Panel();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.splashScreenPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // splashScreenPanel
            // 
            this.splashScreenPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splashScreenPanel.Controls.Add(this.iconPictureBox);
            this.splashScreenPanel.Controls.Add(this.versionLabel);
            this.splashScreenPanel.Controls.Add(this.titleLabel);
            this.splashScreenPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splashScreenPanel.Location = new System.Drawing.Point(10, 10);
            this.splashScreenPanel.Name = "splashScreenPanel";
            this.splashScreenPanel.Padding = new System.Windows.Forms.Padding(10);
            this.splashScreenPanel.Size = new System.Drawing.Size(380, 230);
            this.splashScreenPanel.TabIndex = 0;
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::InfoFenix.Client.Properties.Resources.info_fenix_84x95;
            this.iconPictureBox.Location = new System.Drawing.Point(281, 13);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(84, 95);
            this.iconPictureBox.TabIndex = 2;
            this.iconPictureBox.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Location = new System.Drawing.Point(215, 198);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(150, 20);
            this.versionLabel.TabIndex = 1;
            this.versionLabel.Text = "###########";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.titleLabel.Location = new System.Drawing.Point(13, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(136, 29);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Info Fênix";
            // 
            // SplashScreenForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(400, 250);
            this.Controls.Add(this.splashScreenPanel);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SplashScreenForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info Fênix";
            this.Load += new System.EventHandler(this.SplashScreenForm_Load);
            this.splashScreenPanel.ResumeLayout(false);
            this.splashScreenPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel splashScreenPanel;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox iconPictureBox;
    }
}