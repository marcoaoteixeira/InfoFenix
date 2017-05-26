namespace InfoFenix.Client.Views {
    partial class ProgressForm {
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
            this.mainPanel = new System.Windows.Forms.Panel();
            this.mainProgressBar = new System.Windows.Forms.ProgressBar();
            this.messageLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.progressLabel = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.toggleDetailsLinkLabel = new System.Windows.Forms.LinkLabel();
            this.progressLogRichTextBox = new System.Windows.Forms.RichTextBox();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mainPanel.Controls.Add(this.progressLogRichTextBox);
            this.mainPanel.Controls.Add(this.toggleDetailsLinkLabel);
            this.mainPanel.Controls.Add(this.mainProgressBar);
            this.mainPanel.Controls.Add(this.messageLabel);
            this.mainPanel.Controls.Add(this.titleLabel);
            this.mainPanel.Controls.Add(this.progressLabel);
            this.mainPanel.Controls.Add(this.stopButton);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(10, 10);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Padding = new System.Windows.Forms.Padding(10);
            this.mainPanel.Size = new System.Drawing.Size(480, 390);
            this.mainPanel.TabIndex = 0;
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.mainProgressBar.Location = new System.Drawing.Point(10, 82);
            this.mainProgressBar.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(458, 15);
            this.mainProgressBar.TabIndex = 5;
            // 
            // messageLabel
            // 
            this.messageLabel.AutoEllipsis = true;
            this.messageLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.messageLabel.Location = new System.Drawing.Point(10, 34);
            this.messageLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.messageLabel.Size = new System.Drawing.Size(458, 48);
            this.messageLabel.TabIndex = 6;
            this.messageLabel.Text = "###";
            // 
            // titleLabel
            // 
            this.titleLabel.AutoEllipsis = true;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(10, 10);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(458, 24);
            this.titleLabel.TabIndex = 9;
            this.titleLabel.Text = "###";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressLabel.AutoEllipsis = true;
            this.progressLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.Location = new System.Drawing.Point(10, 113);
            this.progressLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(318, 40);
            this.progressLabel.TabIndex = 8;
            this.progressLabel.Text = "###";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.Location = new System.Drawing.Point(345, 113);
            this.stopButton.Margin = new System.Windows.Forms.Padding(13, 3, 3, 8);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(120, 40);
            this.stopButton.TabIndex = 7;
            this.stopButton.Text = "Parar";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // toggleDetailsLinkLabel
            // 
            this.toggleDetailsLinkLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleDetailsLinkLabel.Location = new System.Drawing.Point(10, 161);
            this.toggleDetailsLinkLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 15);
            this.toggleDetailsLinkLabel.Name = "toggleDetailsLinkLabel";
            this.toggleDetailsLinkLabel.Size = new System.Drawing.Size(451, 16);
            this.toggleDetailsLinkLabel.TabIndex = 10;
            this.toggleDetailsLinkLabel.TabStop = true;
            this.toggleDetailsLinkLabel.Text = "Mais detalhes";
            this.toggleDetailsLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toggleDetailsLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.toggleDetailsLinkLabel_LinkClicked);
            // 
            // progressLogRichTextBox
            // 
            this.progressLogRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.progressLogRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.progressLogRichTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.progressLogRichTextBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLogRichTextBox.Location = new System.Drawing.Point(13, 195);
            this.progressLogRichTextBox.Name = "progressLogRichTextBox";
            this.progressLogRichTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.progressLogRichTextBox.Size = new System.Drawing.Size(452, 180);
            this.progressLogRichTextBox.TabIndex = 11;
            this.progressLogRichTextBox.Text = "";
            // 
            // ProgressForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(500, 410);
            this.Controls.Add(this.mainPanel);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progresso da tarefa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgressForm_FormClosing);
            this.Load += new System.EventHandler(this.ProgressForm_Load);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.ProgressBar mainProgressBar;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.LinkLabel toggleDetailsLinkLabel;
        private System.Windows.Forms.RichTextBox progressLogRichTextBox;
    }
}