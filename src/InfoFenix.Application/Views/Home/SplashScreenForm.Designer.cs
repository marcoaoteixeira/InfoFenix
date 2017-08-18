namespace InfoFenix.Application.Views.Home {
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
            this.messageLabel = new System.Windows.Forms.Label();
            this.progressLabel = new System.Windows.Forms.Label();
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
            this.splashScreenPanel.Controls.Add(this.messageLabel);
            this.splashScreenPanel.Controls.Add(this.progressLabel);
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
            // messageLabel
            // 
            this.messageLabel.AutoEllipsis = true;
            this.messageLabel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageLabel.Location = new System.Drawing.Point(13, 121);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(352, 67);
            this.messageLabel.TabIndex = 4;
            this.messageLabel.Text = "###########";
            // 
            // progressLabel
            // 
            this.progressLabel.AutoEllipsis = true;
            this.progressLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(13, 198);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(150, 20);
            this.progressLabel.TabIndex = 3;
            this.progressLabel.Text = "###########";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = InfoFenix.Resources.Images.info_fenix_84x95;
            this.iconPictureBox.Location = new System.Drawing.Point(281, 13);
            this.iconPictureBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(84, 95);
            this.iconPictureBox.TabIndex = 2;
            this.iconPictureBox.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.titleLabel.Location = new System.Drawing.Point(13, 13);
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SplashScreenForm_FormClosing);
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
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label progressLabel;
    }
}