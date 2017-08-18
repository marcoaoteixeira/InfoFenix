using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using InfoFenix.Resources;

namespace InfoFenix.Application.Views.Shared {
    partial class WaitForForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            waitPanel = new Panel();
            waitLabel = new Label();
            waitPictureBox = new PictureBox();
            waitPanel.SuspendLayout();
            ((ISupportInitialize)(waitPictureBox)).BeginInit();
            SuspendLayout();
            //
            // waitPanel
            //
            waitPanel.BorderStyle = BorderStyle.FixedSingle;
            waitPanel.Controls.Add(waitLabel);
            waitPanel.Controls.Add(waitPictureBox);
            waitPanel.Dock = DockStyle.Fill;
            waitPanel.ForeColor = SystemColors.ControlText;
            waitPanel.Location = new Point(7, 7);
            waitPanel.Name = "waitPanel";
            waitPanel.Padding = new Padding(7);
            waitPanel.Size = new Size(156, 48);
            waitPanel.TabIndex = 0;
            //
            // waitLabel
            //
            waitLabel.Dock = DockStyle.Fill;
            waitLabel.Font = new Font("Tahoma", 9.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            waitLabel.Location = new Point(39, 7);
            waitLabel.Margin = new Padding(0);
            waitLabel.Name = "waitLabel";
            waitLabel.Size = new Size(108, 32);
            waitLabel.TabIndex = 1;
            waitLabel.Text = "Aguarde...";
            waitLabel.TextAlign = ContentAlignment.MiddleCenter;
            //
            // waitPictureBox
            //
            waitPictureBox.Dock = DockStyle.Left;
            waitPictureBox.Image = Images.hourglass_32x32;
            waitPictureBox.Location = new Point(7, 7);
            waitPictureBox.Margin = new Padding(0);
            waitPictureBox.Name = "waitPictureBox";
            waitPictureBox.Size = new Size(32, 32);
            waitPictureBox.TabIndex = 0;
            waitPictureBox.TabStop = false;
            //
            // WaitForm
            //
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(170, 62);
            Controls.Add(waitPanel);
            Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            FormBorderStyle = FormBorderStyle.None;
            Name = "WaitForm";
            Padding = new Padding(7);
            StartPosition = FormStartPosition.CenterScreen;
            waitPanel.ResumeLayout(false);
            ((ISupportInitialize)(waitPictureBox)).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel waitPanel;
        private PictureBox waitPictureBox;
        private Label waitLabel;
    }
}