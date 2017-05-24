namespace InfoFenix.Client.Views {
    partial class ManageDocumentDirectoryForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageDocumentDirectoryForm));
            this.documentDirectorySplitContainer = new System.Windows.Forms.SplitContainer();
            this.directoryTreeView = new System.Windows.Forms.TreeView();
            this.directoryActionPanel = new System.Windows.Forms.Panel();
            this.addDirectoryButton = new System.Windows.Forms.Button();
            this.documentDirectoryListView = new System.Windows.Forms.ListView();
            this.manageDocumentDirectoryImageList = new System.Windows.Forms.ImageList(this.components);
            this.directoryInformationLabel = new System.Windows.Forms.Label();
            this.filterListViewPanel = new System.Windows.Forms.Panel();
            this.filterListViewTextBox = new System.Windows.Forms.TextBox();
            this.filterListViewLabel = new System.Windows.Forms.Label();
            this.directoryTreeViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstDirectoryToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.startWatchDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopWatchDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondDirectoryToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.removeDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentDirectoryListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.indexDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstDocumentToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.removeDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageDocumentDirectoryToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentDirectorySplitContainer)).BeginInit();
            this.documentDirectorySplitContainer.Panel1.SuspendLayout();
            this.documentDirectorySplitContainer.Panel2.SuspendLayout();
            this.documentDirectorySplitContainer.SuspendLayout();
            this.directoryActionPanel.SuspendLayout();
            this.filterListViewPanel.SuspendLayout();
            this.directoryTreeViewContextMenuStrip.SuspendLayout();
            this.documentDirectoryListViewContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::InfoFenix.Client.Properties.Resources.document_directory_128x128;
            // 
            // titleLabel
            // 
            this.titleLabel.Text = "Gerenciar diretório de documentos";
            // 
            // subTitleLabel
            // 
            this.subTitleLabel.Text = "Nessa tela é possível, cadastrar, editar e remover diretórios de documentos.";
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.documentDirectorySplitContainer);
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0);
            // 
            // closeButton
            // 
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // documentDirectorySplitContainer
            // 
            this.documentDirectorySplitContainer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.documentDirectorySplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentDirectorySplitContainer.Location = new System.Drawing.Point(0, 0);
            this.documentDirectorySplitContainer.Name = "documentDirectorySplitContainer";
            // 
            // documentDirectorySplitContainer.Panel1
            // 
            this.documentDirectorySplitContainer.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.documentDirectorySplitContainer.Panel1.Controls.Add(this.directoryTreeView);
            this.documentDirectorySplitContainer.Panel1.Controls.Add(this.directoryActionPanel);
            this.documentDirectorySplitContainer.Panel1.Padding = new System.Windows.Forms.Padding(10);
            this.documentDirectorySplitContainer.Panel1MinSize = 270;
            // 
            // documentDirectorySplitContainer.Panel2
            // 
            this.documentDirectorySplitContainer.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.documentDirectorySplitContainer.Panel2.Controls.Add(this.documentDirectoryListView);
            this.documentDirectorySplitContainer.Panel2.Controls.Add(this.directoryInformationLabel);
            this.documentDirectorySplitContainer.Panel2.Controls.Add(this.filterListViewPanel);
            this.documentDirectorySplitContainer.Panel2.Padding = new System.Windows.Forms.Padding(10);
            this.documentDirectorySplitContainer.Size = new System.Drawing.Size(784, 381);
            this.documentDirectorySplitContainer.SplitterDistance = 270;
            this.documentDirectorySplitContainer.TabIndex = 0;
            // 
            // directoryTreeView
            // 
            this.directoryTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.directoryTreeView.Location = new System.Drawing.Point(10, 68);
            this.directoryTreeView.Name = "directoryTreeView";
            this.directoryTreeView.Size = new System.Drawing.Size(250, 303);
            this.directoryTreeView.TabIndex = 1;
            this.directoryTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.directoryTreeView_NodeMouseClick);
            this.directoryTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.directoryTreeView_MouseUp);
            // 
            // directoryActionPanel
            // 
            this.directoryActionPanel.Controls.Add(this.addDirectoryButton);
            this.directoryActionPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.directoryActionPanel.Location = new System.Drawing.Point(10, 10);
            this.directoryActionPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.directoryActionPanel.Name = "directoryActionPanel";
            this.directoryActionPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.directoryActionPanel.Size = new System.Drawing.Size(250, 58);
            this.directoryActionPanel.TabIndex = 0;
            // 
            // addDirectoryButton
            // 
            this.addDirectoryButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.addDirectoryButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addDirectoryButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.addDirectoryButton.Image = global::InfoFenix.Client.Properties.Resources.add_folder_32x32;
            this.addDirectoryButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.addDirectoryButton.Location = new System.Drawing.Point(0, 0);
            this.addDirectoryButton.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.addDirectoryButton.Name = "addDirectoryButton";
            this.addDirectoryButton.Padding = new System.Windows.Forms.Padding(10);
            this.addDirectoryButton.Size = new System.Drawing.Size(250, 48);
            this.addDirectoryButton.TabIndex = 0;
            this.addDirectoryButton.Text = "Adicionar diretório";
            this.manageDocumentDirectoryToolTip.SetToolTip(this.addDirectoryButton, "Adicionar diretório de documentos");
            this.addDirectoryButton.UseVisualStyleBackColor = true;
            this.addDirectoryButton.Click += new System.EventHandler(this.addDirectoryButton_Click);
            // 
            // documentDirectoryListView
            // 
            this.documentDirectoryListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentDirectoryListView.LargeImageList = this.manageDocumentDirectoryImageList;
            this.documentDirectoryListView.Location = new System.Drawing.Point(10, 68);
            this.documentDirectoryListView.Name = "documentDirectoryListView";
            this.documentDirectoryListView.Size = new System.Drawing.Size(490, 271);
            this.documentDirectoryListView.TabIndex = 0;
            this.documentDirectoryListView.UseCompatibleStateImageBehavior = false;
            this.documentDirectoryListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.documentDirectoryListView_MouseDown);
            // 
            // manageDocumentDirectoryImageList
            // 
            this.manageDocumentDirectoryImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("manageDocumentDirectoryImageList.ImageStream")));
            this.manageDocumentDirectoryImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.manageDocumentDirectoryImageList.Images.SetKeyName(0, "folder_32x32.png");
            this.manageDocumentDirectoryImageList.Images.SetKeyName(1, "word-document-doc_blue_32x32.png");
            this.manageDocumentDirectoryImageList.Images.SetKeyName(2, "word-document-doc_gray_32x32.png");
            this.manageDocumentDirectoryImageList.Images.SetKeyName(3, "word-document-docx_blue_32x32.png");
            this.manageDocumentDirectoryImageList.Images.SetKeyName(4, "word-document-docx_gray_32x32.png");
            // 
            // directoryInformationLabel
            // 
            this.directoryInformationLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.directoryInformationLabel.Location = new System.Drawing.Point(10, 339);
            this.directoryInformationLabel.Name = "directoryInformationLabel";
            this.directoryInformationLabel.Padding = new System.Windows.Forms.Padding(5);
            this.directoryInformationLabel.Size = new System.Drawing.Size(490, 32);
            this.directoryInformationLabel.TabIndex = 2;
            this.directoryInformationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // filterListViewPanel
            // 
            this.filterListViewPanel.Controls.Add(this.filterListViewTextBox);
            this.filterListViewPanel.Controls.Add(this.filterListViewLabel);
            this.filterListViewPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterListViewPanel.Location = new System.Drawing.Point(10, 10);
            this.filterListViewPanel.Name = "filterListViewPanel";
            this.filterListViewPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.filterListViewPanel.Size = new System.Drawing.Size(490, 58);
            this.filterListViewPanel.TabIndex = 1;
            // 
            // filterListViewTextBox
            // 
            this.filterListViewTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterListViewTextBox.Location = new System.Drawing.Point(70, 12);
            this.filterListViewTextBox.Name = "filterListViewTextBox";
            this.filterListViewTextBox.Size = new System.Drawing.Size(410, 27);
            this.filterListViewTextBox.TabIndex = 1;
            this.filterListViewTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.filterListViewTextBox_KeyDown);
            // 
            // filterListViewLabel
            // 
            this.filterListViewLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.filterListViewLabel.Location = new System.Drawing.Point(0, 0);
            this.filterListViewLabel.Name = "filterListViewLabel";
            this.filterListViewLabel.Size = new System.Drawing.Size(64, 48);
            this.filterListViewLabel.TabIndex = 0;
            this.filterListViewLabel.Text = "Filtrar:";
            this.filterListViewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // directoryTreeViewContextMenuStrip
            // 
            this.directoryTreeViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editDirectoryToolStripMenuItem,
            this.indexDirectoryToolStripMenuItem,
            this.firstDirectoryToolStripSeparator,
            this.startWatchDirectoryToolStripMenuItem,
            this.stopWatchDirectoryToolStripMenuItem,
            this.secondDirectoryToolStripSeparator,
            this.removeDirectoryToolStripMenuItem});
            this.directoryTreeViewContextMenuStrip.Name = "directoryTreeViewContextMenuStrip";
            this.directoryTreeViewContextMenuStrip.Size = new System.Drawing.Size(170, 126);
            // 
            // editDirectoryToolStripMenuItem
            // 
            this.editDirectoryToolStripMenuItem.Name = "editDirectoryToolStripMenuItem";
            this.editDirectoryToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.editDirectoryToolStripMenuItem.Text = "Editar";
            // 
            // indexDirectoryToolStripMenuItem
            // 
            this.indexDirectoryToolStripMenuItem.Name = "indexDirectoryToolStripMenuItem";
            this.indexDirectoryToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.indexDirectoryToolStripMenuItem.Text = "Indexar";
            // 
            // firstDirectoryToolStripSeparator
            // 
            this.firstDirectoryToolStripSeparator.Name = "firstDirectoryToolStripSeparator";
            this.firstDirectoryToolStripSeparator.Size = new System.Drawing.Size(166, 6);
            // 
            // startWatchDirectoryToolStripMenuItem
            // 
            this.startWatchDirectoryToolStripMenuItem.Name = "startWatchDirectoryToolStripMenuItem";
            this.startWatchDirectoryToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.startWatchDirectoryToolStripMenuItem.Text = "Iniciar observação";
            // 
            // stopWatchDirectoryToolStripMenuItem
            // 
            this.stopWatchDirectoryToolStripMenuItem.Name = "stopWatchDirectoryToolStripMenuItem";
            this.stopWatchDirectoryToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.stopWatchDirectoryToolStripMenuItem.Text = "Parar observação";
            this.stopWatchDirectoryToolStripMenuItem.Visible = false;
            // 
            // secondDirectoryToolStripSeparator
            // 
            this.secondDirectoryToolStripSeparator.Name = "secondDirectoryToolStripSeparator";
            this.secondDirectoryToolStripSeparator.Size = new System.Drawing.Size(166, 6);
            // 
            // removeDirectoryToolStripMenuItem
            // 
            this.removeDirectoryToolStripMenuItem.Name = "removeDirectoryToolStripMenuItem";
            this.removeDirectoryToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.removeDirectoryToolStripMenuItem.Text = "Remover";
            this.removeDirectoryToolStripMenuItem.Click += new System.EventHandler(this.removeDirectoryToolStripMenuItem_Click);
            // 
            // documentDirectoryListViewContextMenuStrip
            // 
            this.documentDirectoryListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexDocumentToolStripMenuItem,
            this.firstDocumentToolStripSeparator,
            this.removeDocumentToolStripMenuItem});
            this.documentDirectoryListViewContextMenuStrip.Name = "documentDirectoryListViewContextMenuStrip";
            this.documentDirectoryListViewContextMenuStrip.Size = new System.Drawing.Size(122, 54);
            // 
            // indexDocumentToolStripMenuItem
            // 
            this.indexDocumentToolStripMenuItem.Name = "indexDocumentToolStripMenuItem";
            this.indexDocumentToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.indexDocumentToolStripMenuItem.Text = "Indexar";
            // 
            // firstDocumentToolStripSeparator
            // 
            this.firstDocumentToolStripSeparator.Name = "firstDocumentToolStripSeparator";
            this.firstDocumentToolStripSeparator.Size = new System.Drawing.Size(118, 6);
            // 
            // removeDocumentToolStripMenuItem
            // 
            this.removeDocumentToolStripMenuItem.Name = "removeDocumentToolStripMenuItem";
            this.removeDocumentToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.removeDocumentToolStripMenuItem.Text = "Remover";
            // 
            // ManageDocumentDirectoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Name = "ManageDocumentDirectoryForm";
            this.Text = "Gerenciar diretórios de documento";
            this.Load += new System.EventHandler(this.ManageDocumentDirectoryForm_Load);
            this.Controls.SetChildIndex(this.titlePanel, 0);
            this.Controls.SetChildIndex(this.topSeparator, 0);
            this.Controls.SetChildIndex(this.actionPanel, 0);
            this.Controls.SetChildIndex(this.bottomSeparator, 0);
            this.Controls.SetChildIndex(this.contentPanel, 0);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.documentDirectorySplitContainer.Panel1.ResumeLayout(false);
            this.documentDirectorySplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentDirectorySplitContainer)).EndInit();
            this.documentDirectorySplitContainer.ResumeLayout(false);
            this.directoryActionPanel.ResumeLayout(false);
            this.filterListViewPanel.ResumeLayout(false);
            this.filterListViewPanel.PerformLayout();
            this.directoryTreeViewContextMenuStrip.ResumeLayout(false);
            this.documentDirectoryListViewContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer documentDirectorySplitContainer;
        private System.Windows.Forms.TreeView directoryTreeView;
        private System.Windows.Forms.Panel directoryActionPanel;
        private System.Windows.Forms.Button addDirectoryButton;
        private System.Windows.Forms.ListView documentDirectoryListView;
        private System.Windows.Forms.ContextMenuStrip directoryTreeViewContextMenuStrip;
        private System.Windows.Forms.ContextMenuStrip documentDirectoryListViewContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator firstDirectoryToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem removeDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator firstDocumentToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem removeDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startWatchDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopWatchDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator secondDirectoryToolStripSeparator;
        private System.Windows.Forms.ToolTip manageDocumentDirectoryToolTip;
        private System.Windows.Forms.ImageList manageDocumentDirectoryImageList;
        private System.Windows.Forms.Panel filterListViewPanel;
        private System.Windows.Forms.TextBox filterListViewTextBox;
        private System.Windows.Forms.Label filterListViewLabel;
        private System.Windows.Forms.Label directoryInformationLabel;
    }
}