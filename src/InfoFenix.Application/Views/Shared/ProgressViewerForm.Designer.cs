using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Application.Views.Shared;
using InfoFenix;
using InfoFenix.CQRS;

namespace InfoFenix.Application.Views.Shared {
    partial class ProgressViewerForm {
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
            progressTitleLabel = new Label();
            progressMessageLabel = new Label();
            mainProgressBar = new ProgressBar();
            titlePanel.SuspendLayout();
            ((ISupportInitialize)(iconPictureBox)).BeginInit();
            contentPanel.SuspendLayout();
            actionPanel.SuspendLayout();
            SuspendLayout();
            //
            // contentPanel
            //
            contentPanel.Controls.Add(mainProgressBar);
            contentPanel.Controls.Add(progressMessageLabel);
            contentPanel.Controls.Add(progressTitleLabel);
            contentPanel.Location = new Point(0, 0);
            contentPanel.Padding = new Padding(15);
            contentPanel.Size = new Size(434, 139);
            //
            // bottomSeparator
            //
            bottomSeparator.Location = new Point(0, 139);
            bottomSeparator.Size = new Size(434, 2);
            //
            // actionPanel
            //
            actionPanel.Location = new Point(0, 141);
            actionPanel.Size = new Size(434, 80);
            //
            // closeButton
            //
            closeButton.Location = new Point(294, 20);
            closeButton.Click += new EventHandler(closeButton_Click);
            //
            // progressTitleLabel
            //
            progressTitleLabel.AutoEllipsis = true;
            progressTitleLabel.Dock = DockStyle.Top;
            progressTitleLabel.Font = new Font("Tahoma", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            progressTitleLabel.Location = new Point(15, 15);
            progressTitleLabel.Name = "progressTitleLabel";
            progressTitleLabel.Size = new Size(404, 32);
            progressTitleLabel.TabIndex = 0;
            progressTitleLabel.Text = "###";
            progressTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
            //
            // progressMessageLabel
            //
            progressMessageLabel.AutoEllipsis = true;
            progressMessageLabel.Dock = DockStyle.Top;
            progressMessageLabel.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            progressMessageLabel.Location = new Point(15, 47);
            progressMessageLabel.Name = "progressMessageLabel";
            progressMessageLabel.Padding = new Padding(0, 10, 0, 10);
            progressMessageLabel.Size = new Size(404, 60);
            progressMessageLabel.TabIndex = 1;
            progressMessageLabel.Text = "###";
            //
            // mainProgressBar
            //
            mainProgressBar.Dock = DockStyle.Fill;
            mainProgressBar.Location = new Point(15, 107);
            mainProgressBar.Name = "mainProgressBar";
            mainProgressBar.Size = new Size(404, 17);
            mainProgressBar.TabIndex = 2;
            //
            // ProgressiveTaskForm
            //
            AutoScaleDimensions = new SizeF(10F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(434, 221);
            ControlBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "ProgressiveTaskForm";
            ShowHeader = false;
            Text = "Executando tarefa...";
            FormClosing += new FormClosingEventHandler(ProgressViewerForm_FormClosing);
            Load += new EventHandler(ProgressViewerForm_Load);
            titlePanel.ResumeLayout(false);
            ((ISupportInitialize)(iconPictureBox)).EndInit();
            contentPanel.ResumeLayout(false);
            actionPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion Windows Form Designer generated code

        private Label progressTitleLabel;
        private ProgressBar mainProgressBar;
        private Label progressMessageLabel;
    }
}