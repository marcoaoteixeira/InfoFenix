namespace InfoFenix.Application.Views.DocumentDirectory {
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
            this.documentDirectoryContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editDocumentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.indexDocumentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstDocumentDirectoryToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.cleanDocumentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.secondDocumentDirectoryToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.removeDocumentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentDirectoryDataGridView = new System.Windows.Forms.DataGridView();
            this.labelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newDocumentDirectoryButton = new System.Windows.Forms.Button();
            this.topButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.bottomButton = new System.Windows.Forms.Button();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.documentDirectoryContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentDirectoryDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = InfoFenix.Resources.Images.document_directory_128x128;
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
            this.contentPanel.Controls.Add(this.bottomButton);
            this.contentPanel.Controls.Add(this.downButton);
            this.contentPanel.Controls.Add(this.upButton);
            this.contentPanel.Controls.Add(this.topButton);
            this.contentPanel.Controls.Add(this.newDocumentDirectoryButton);
            this.contentPanel.Controls.Add(this.documentDirectoryDataGridView);
            this.contentPanel.Size = new System.Drawing.Size(784, 286);
            // 
            // bottomSeparator
            // 
            this.bottomSeparator.Location = new System.Drawing.Point(0, 384);
            // 
            // actionPanel
            // 
            this.actionPanel.Location = new System.Drawing.Point(0, 386);
            // 
            // closeButton
            // 
            this.closeButton.TabIndex = 2;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // documentDirectoryContextMenuStrip
            // 
            this.documentDirectoryContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editDocumentDirectoryToolStripMenuItem,
            this.indexDocumentDirectoryToolStripMenuItem,
            this.firstDocumentDirectoryToolStripSeparator,
            this.cleanDocumentDirectoryToolStripMenuItem,
            this.secondDocumentDirectoryToolStripSeparator,
            this.removeDocumentDirectoryToolStripMenuItem});
            this.documentDirectoryContextMenuStrip.Name = "directoryTreeViewContextMenuStrip";
            this.documentDirectoryContextMenuStrip.ShowImageMargin = false;
            this.documentDirectoryContextMenuStrip.Size = new System.Drawing.Size(231, 104);
            // 
            // editDocumentDirectoryToolStripMenuItem
            // 
            this.editDocumentDirectoryToolStripMenuItem.Name = "editDocumentDirectoryToolStripMenuItem";
            this.editDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.editDocumentDirectoryToolStripMenuItem.Tag = "EDIT";
            this.editDocumentDirectoryToolStripMenuItem.Text = "Editar diretório de documentos";
            this.editDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // indexDocumentDirectoryToolStripMenuItem
            // 
            this.indexDocumentDirectoryToolStripMenuItem.Name = "indexDocumentDirectoryToolStripMenuItem";
            this.indexDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.indexDocumentDirectoryToolStripMenuItem.Tag = "INDEX";
            this.indexDocumentDirectoryToolStripMenuItem.Text = "Atualizar indíce";
            this.indexDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // firstDocumentDirectoryToolStripSeparator
            // 
            this.firstDocumentDirectoryToolStripSeparator.Name = "firstDocumentDirectoryToolStripSeparator";
            this.firstDocumentDirectoryToolStripSeparator.Size = new System.Drawing.Size(227, 6);
            // 
            // cleanDocumentDirectoryToolStripMenuItem
            // 
            this.cleanDocumentDirectoryToolStripMenuItem.Name = "cleanDocumentDirectoryToolStripMenuItem";
            this.cleanDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.cleanDocumentDirectoryToolStripMenuItem.Tag = "CLEAN";
            this.cleanDocumentDirectoryToolStripMenuItem.Text = "Atualizar documentos";
            this.cleanDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // secondDocumentDirectoryToolStripSeparator
            // 
            this.secondDocumentDirectoryToolStripSeparator.Name = "secondDocumentDirectoryToolStripSeparator";
            this.secondDocumentDirectoryToolStripSeparator.Size = new System.Drawing.Size(227, 6);
            // 
            // removeDocumentDirectoryToolStripMenuItem
            // 
            this.removeDocumentDirectoryToolStripMenuItem.Name = "removeDocumentDirectoryToolStripMenuItem";
            this.removeDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.removeDocumentDirectoryToolStripMenuItem.Tag = "REMOVE";
            this.removeDocumentDirectoryToolStripMenuItem.Text = "Remover diretório de documentos";
            this.removeDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // documentDirectoryDataGridView
            // 
            this.documentDirectoryDataGridView.AllowUserToAddRows = false;
            this.documentDirectoryDataGridView.AllowUserToDeleteRows = false;
            this.documentDirectoryDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentDirectoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.documentDirectoryDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.labelDataGridViewTextBoxColumn,
            this.pathDataGridViewTextBoxColumn});
            this.documentDirectoryDataGridView.Cursor = System.Windows.Forms.Cursors.Default;
            this.documentDirectoryDataGridView.Location = new System.Drawing.Point(10, 10);
            this.documentDirectoryDataGridView.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.documentDirectoryDataGridView.MultiSelect = false;
            this.documentDirectoryDataGridView.Name = "documentDirectoryDataGridView";
            this.documentDirectoryDataGridView.ReadOnly = true;
            this.documentDirectoryDataGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.documentDirectoryDataGridView.RowTemplate.Height = 32;
            this.documentDirectoryDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.documentDirectoryDataGridView.Size = new System.Drawing.Size(713, 200);
            this.documentDirectoryDataGridView.TabIndex = 0;
            this.documentDirectoryDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.documentDirectoryDataGridView_MouseClick);
            // 
            // labelDataGridViewTextBoxColumn
            // 
            this.labelDataGridViewTextBoxColumn.DataPropertyName = "Label";
            this.labelDataGridViewTextBoxColumn.HeaderText = "Rótulo";
            this.labelDataGridViewTextBoxColumn.Name = "labelDataGridViewTextBoxColumn";
            this.labelDataGridViewTextBoxColumn.ReadOnly = true;
            this.labelDataGridViewTextBoxColumn.Width = 300;
            // 
            // pathDataGridViewTextBoxColumn
            // 
            this.pathDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.pathDataGridViewTextBoxColumn.DataPropertyName = "Path";
            this.pathDataGridViewTextBoxColumn.HeaderText = "Caminho do Diretório";
            this.pathDataGridViewTextBoxColumn.Name = "pathDataGridViewTextBoxColumn";
            this.pathDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // newDocumentDirectoryButton
            // 
            this.newDocumentDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newDocumentDirectoryButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.newDocumentDirectoryButton.Location = new System.Drawing.Point(10, 226);
            this.newDocumentDirectoryButton.Name = "newDocumentDirectoryButton";
            this.newDocumentDirectoryButton.Size = new System.Drawing.Size(300, 48);
            this.newDocumentDirectoryButton.TabIndex = 1;
            this.newDocumentDirectoryButton.Text = "Novo Diretório de Documentos";
            this.newDocumentDirectoryButton.UseVisualStyleBackColor = true;
            this.newDocumentDirectoryButton.Click += new System.EventHandler(this.newDocumentDirectoryButton_Click);
            // 
            // topButton
            // 
            this.topButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.topButton.Image = InfoFenix.Resources.Images.top_24x24;
            this.topButton.Location = new System.Drawing.Point(739, 13);
            this.topButton.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
            this.topButton.Name = "topButton";
            this.topButton.Size = new System.Drawing.Size(32, 32);
            this.topButton.TabIndex = 2;
            this.topButton.Tag = "TOP";
            this.topButton.UseVisualStyleBackColor = true;
            this.topButton.Click += new System.EventHandler(this.ChangePosition_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = InfoFenix.Resources.Images.up_24x24;
            this.upButton.Location = new System.Drawing.Point(739, 51);
            this.upButton.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(32, 32);
            this.upButton.TabIndex = 3;
            this.upButton.Tag = "UP";
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.ChangePosition_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = InfoFenix.Resources.Images.down_24x24;
            this.downButton.Location = new System.Drawing.Point(739, 89);
            this.downButton.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(32, 32);
            this.downButton.TabIndex = 4;
            this.downButton.Tag = "DOWN";
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.ChangePosition_Click);
            // 
            // bottomButton
            // 
            this.bottomButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomButton.Image = InfoFenix.Resources.Images.bottom_24x24;
            this.bottomButton.Location = new System.Drawing.Point(739, 127);
            this.bottomButton.Margin = new System.Windows.Forms.Padding(13, 3, 3, 3);
            this.bottomButton.Name = "bottomButton";
            this.bottomButton.Size = new System.Drawing.Size(32, 32);
            this.bottomButton.TabIndex = 5;
            this.bottomButton.Tag = "BOTTOM";
            this.bottomButton.UseVisualStyleBackColor = true;
            this.bottomButton.Click += new System.EventHandler(this.ChangePosition_Click);
            // 
            // ManageDocumentDirectoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 466);
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
            this.documentDirectoryContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentDirectoryDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip documentDirectoryContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem editDocumentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem indexDocumentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDocumentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator firstDocumentDirectoryToolStripSeparator;
        private System.Windows.Forms.Button newDocumentDirectoryButton;
        private System.Windows.Forms.DataGridView documentDirectoryDataGridView;
        private System.Windows.Forms.ToolStripMenuItem cleanDocumentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator secondDocumentDirectoryToolStripSeparator;
        private System.Windows.Forms.Button bottomButton;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button topButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn labelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
    }
}