namespace InfoFenix.Client.Views.Search {
    partial class SearchResultForm {
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
            this.components = new System.ComponentModel.Container();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.informationPanel = new System.Windows.Forms.Panel();
            this.informationLabel = new System.Windows.Forms.Label();
            this.copyTextButton = new System.Windows.Forms.Button();
            this.goToLastButton = new System.Windows.Forms.Button();
            this.goToNextButton = new System.Windows.Forms.Button();
            this.goToPreviousButton = new System.Windows.Forms.Button();
            this.goToFirstButton = new System.Windows.Forms.Button();
            this.documentViewerPanel = new System.Windows.Forms.Panel();
            this.spacerPanel = new System.Windows.Forms.Panel();
            this.documentViewerRichTextBox = new System.Windows.Forms.RichTextBox();
            this.documentViewerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.documentPathLabel = new System.Windows.Forms.Label();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.controlPanel.SuspendLayout();
            this.informationPanel.SuspendLayout();
            this.documentViewerPanel.SuspendLayout();
            this.spacerPanel.SuspendLayout();
            this.documentViewerContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.Size = new System.Drawing.Size(634, 96);
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::InfoFenix.Client.Properties.Resources.search_result_128x128;
            // 
            // titleLabel
            // 
            this.titleLabel.Size = new System.Drawing.Size(538, 56);
            this.titleLabel.Text = "Resultado da Pesquisa";
            // 
            // subTitleLabel
            // 
            this.subTitleLabel.Size = new System.Drawing.Size(538, 40);
            this.subTitleLabel.Text = "Exibe todos os documentos que foram encontrados pela pesquisa.";
            // 
            // topSeparator
            // 
            this.topSeparator.Size = new System.Drawing.Size(634, 2);
            // 
            // contentPanel
            // 
            this.contentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.contentPanel.Controls.Add(this.documentViewerPanel);
            this.contentPanel.Controls.Add(this.controlPanel);
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0);
            this.contentPanel.Size = new System.Drawing.Size(634, 281);
            // 
            // bottomSeparator
            // 
            this.bottomSeparator.Location = new System.Drawing.Point(0, 379);
            this.bottomSeparator.Size = new System.Drawing.Size(634, 2);
            // 
            // actionPanel
            // 
            this.actionPanel.Controls.Add(this.documentPathLabel);
            this.actionPanel.Location = new System.Drawing.Point(0, 381);
            this.actionPanel.Size = new System.Drawing.Size(634, 80);
            this.actionPanel.Controls.SetChildIndex(this.closeButton, 0);
            this.actionPanel.Controls.SetChildIndex(this.documentPathLabel, 0);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(494, 20);
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // controlPanel
            // 
            this.controlPanel.BackColor = System.Drawing.SystemColors.Control;
            this.controlPanel.Controls.Add(this.informationPanel);
            this.controlPanel.Controls.Add(this.copyTextButton);
            this.controlPanel.Controls.Add(this.goToLastButton);
            this.controlPanel.Controls.Add(this.goToNextButton);
            this.controlPanel.Controls.Add(this.goToPreviousButton);
            this.controlPanel.Controls.Add(this.goToFirstButton);
            this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Padding = new System.Windows.Forms.Padding(5);
            this.controlPanel.Size = new System.Drawing.Size(634, 80);
            this.controlPanel.TabIndex = 0;
            // 
            // informationPanel
            // 
            this.informationPanel.Controls.Add(this.informationLabel);
            this.informationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.informationPanel.Location = new System.Drawing.Point(380, 5);
            this.informationPanel.Name = "informationPanel";
            this.informationPanel.Padding = new System.Windows.Forms.Padding(15);
            this.informationPanel.Size = new System.Drawing.Size(249, 70);
            this.informationPanel.TabIndex = 5;
            // 
            // informationLabel
            // 
            this.informationLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.informationLabel.Location = new System.Drawing.Point(18, 15);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Size = new System.Drawing.Size(213, 40);
            this.informationLabel.TabIndex = 6;
            this.informationLabel.Text = "###";
            this.informationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // copyTextButton
            // 
            this.copyTextButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.copyTextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.copyTextButton.Image = global::InfoFenix.Client.Properties.Resources.copy_64x64;
            this.copyTextButton.Location = new System.Drawing.Point(305, 5);
            this.copyTextButton.Name = "copyTextButton";
            this.copyTextButton.Size = new System.Drawing.Size(75, 70);
            this.copyTextButton.TabIndex = 4;
            this.mainToolTip.SetToolTip(this.copyTextButton, "Copiar texto");
            this.copyTextButton.UseVisualStyleBackColor = true;
            this.copyTextButton.Click += new System.EventHandler(this.copyTextButton_Click);
            // 
            // goToLastButton
            // 
            this.goToLastButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goToLastButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.goToLastButton.Image = global::InfoFenix.Client.Properties.Resources.fast_forward_64x64;
            this.goToLastButton.Location = new System.Drawing.Point(230, 5);
            this.goToLastButton.Name = "goToLastButton";
            this.goToLastButton.Size = new System.Drawing.Size(75, 70);
            this.goToLastButton.TabIndex = 3;
            this.mainToolTip.SetToolTip(this.goToLastButton, "Último");
            this.goToLastButton.UseVisualStyleBackColor = true;
            this.goToLastButton.Click += new System.EventHandler(this.lastResultButton_Click);
            // 
            // goToNextButton
            // 
            this.goToNextButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goToNextButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.goToNextButton.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.goToNextButton.Image = global::InfoFenix.Client.Properties.Resources.forward_64x64;
            this.goToNextButton.Location = new System.Drawing.Point(155, 5);
            this.goToNextButton.Name = "goToNextButton";
            this.goToNextButton.Size = new System.Drawing.Size(75, 70);
            this.goToNextButton.TabIndex = 2;
            this.goToNextButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.mainToolTip.SetToolTip(this.goToNextButton, "Próximo");
            this.goToNextButton.UseVisualStyleBackColor = true;
            this.goToNextButton.Click += new System.EventHandler(this.nextResultButton_Click);
            // 
            // goToPreviousButton
            // 
            this.goToPreviousButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goToPreviousButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.goToPreviousButton.Image = global::InfoFenix.Client.Properties.Resources.rewind_64x64;
            this.goToPreviousButton.Location = new System.Drawing.Point(80, 5);
            this.goToPreviousButton.Name = "goToPreviousButton";
            this.goToPreviousButton.Size = new System.Drawing.Size(75, 70);
            this.goToPreviousButton.TabIndex = 1;
            this.mainToolTip.SetToolTip(this.goToPreviousButton, "Anterior");
            this.goToPreviousButton.UseVisualStyleBackColor = true;
            this.goToPreviousButton.Click += new System.EventHandler(this.previousResultButton_Click);
            // 
            // goToFirstButton
            // 
            this.goToFirstButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.goToFirstButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.goToFirstButton.Image = global::InfoFenix.Client.Properties.Resources.fast_rewind_64x64;
            this.goToFirstButton.Location = new System.Drawing.Point(5, 5);
            this.goToFirstButton.Name = "goToFirstButton";
            this.goToFirstButton.Size = new System.Drawing.Size(75, 70);
            this.goToFirstButton.TabIndex = 0;
            this.mainToolTip.SetToolTip(this.goToFirstButton, "Primeiro");
            this.goToFirstButton.UseVisualStyleBackColor = true;
            this.goToFirstButton.Click += new System.EventHandler(this.firstResultButton_Click);
            // 
            // documentViewerPanel
            // 
            this.documentViewerPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.documentViewerPanel.Controls.Add(this.spacerPanel);
            this.documentViewerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentViewerPanel.Location = new System.Drawing.Point(0, 80);
            this.documentViewerPanel.Name = "documentViewerPanel";
            this.documentViewerPanel.Padding = new System.Windows.Forms.Padding(100, 10, 100, 10);
            this.documentViewerPanel.Size = new System.Drawing.Size(634, 201);
            this.documentViewerPanel.TabIndex = 2;
            // 
            // spacerPanel
            // 
            this.spacerPanel.BackColor = System.Drawing.SystemColors.Window;
            this.spacerPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.spacerPanel.Controls.Add(this.documentViewerRichTextBox);
            this.spacerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spacerPanel.Location = new System.Drawing.Point(100, 10);
            this.spacerPanel.Name = "spacerPanel";
            this.spacerPanel.Padding = new System.Windows.Forms.Padding(15, 15, 0, 15);
            this.spacerPanel.Size = new System.Drawing.Size(434, 181);
            this.spacerPanel.TabIndex = 0;
            // 
            // documentViewerRichTextBox
            // 
            this.documentViewerRichTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.documentViewerRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.documentViewerRichTextBox.ContextMenuStrip = this.documentViewerContextMenuStrip;
            this.documentViewerRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentViewerRichTextBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.documentViewerRichTextBox.Location = new System.Drawing.Point(15, 15);
            this.documentViewerRichTextBox.Name = "documentViewerRichTextBox";
            this.documentViewerRichTextBox.ReadOnly = true;
            this.documentViewerRichTextBox.Size = new System.Drawing.Size(415, 147);
            this.documentViewerRichTextBox.TabIndex = 0;
            this.documentViewerRichTextBox.Text = "";
            // 
            // documentViewerContextMenuStrip
            // 
            this.documentViewerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyTextToolStripMenuItem});
            this.documentViewerContextMenuStrip.Name = "documentViewerContextMenuStrip";
            this.documentViewerContextMenuStrip.Size = new System.Drawing.Size(110, 26);
            // 
            // copyTextToolStripMenuItem
            // 
            this.copyTextToolStripMenuItem.Image = global::InfoFenix.Client.Properties.Resources.copy_text_16x16;
            this.copyTextToolStripMenuItem.Name = "copyTextToolStripMenuItem";
            this.copyTextToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.copyTextToolStripMenuItem.Text = "Copiar";
            this.copyTextToolStripMenuItem.Click += new System.EventHandler(this.copyTextToolStripMenuItem_Click);
            // 
            // documentPathLabel
            // 
            this.documentPathLabel.AutoEllipsis = true;
            this.documentPathLabel.Location = new System.Drawing.Point(12, 20);
            this.documentPathLabel.Name = "documentPathLabel";
            this.documentPathLabel.Size = new System.Drawing.Size(476, 40);
            this.documentPathLabel.TabIndex = 1;
            this.documentPathLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SearchResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 461);
            this.Name = "SearchResultForm";
            this.Text = "Resultado da Pesquisa";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchResultForm_FormClosing);
            this.Load += new System.EventHandler(this.SearchResultForm_Load);
            this.Controls.SetChildIndex(this.titlePanel, 0);
            this.Controls.SetChildIndex(this.topSeparator, 0);
            this.Controls.SetChildIndex(this.actionPanel, 0);
            this.Controls.SetChildIndex(this.bottomSeparator, 0);
            this.Controls.SetChildIndex(this.contentPanel, 0);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.controlPanel.ResumeLayout(false);
            this.informationPanel.ResumeLayout(false);
            this.documentViewerPanel.ResumeLayout(false);
            this.spacerPanel.ResumeLayout(false);
            this.documentViewerContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel documentViewerPanel;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button goToFirstButton;
        private System.Windows.Forms.Button copyTextButton;
        private System.Windows.Forms.Button goToLastButton;
        private System.Windows.Forms.Button goToNextButton;
        private System.Windows.Forms.Button goToPreviousButton;
        private System.Windows.Forms.Panel informationPanel;
        private System.Windows.Forms.Label informationLabel;
        private System.Windows.Forms.ToolTip mainToolTip;
        private System.Windows.Forms.Label documentPathLabel;
        private System.Windows.Forms.Panel spacerPanel;
        private System.Windows.Forms.RichTextBox documentViewerRichTextBox;
        private System.Windows.Forms.ContextMenuStrip documentViewerContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyTextToolStripMenuItem;
    }
}