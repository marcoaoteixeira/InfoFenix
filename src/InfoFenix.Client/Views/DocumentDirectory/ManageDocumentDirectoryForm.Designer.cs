namespace InfoFenix.Client.Views.DocumentDirectory {
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
            this.positionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.labelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pathDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newDocumentDirectoryButton = new System.Windows.Forms.Button();
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
            this.documentDirectoryContextMenuStrip.Size = new System.Drawing.Size(97, 104);
            // 
            // editDocumentDirectoryToolStripMenuItem
            // 
            this.editDocumentDirectoryToolStripMenuItem.Name = "editDocumentDirectoryToolStripMenuItem";
            this.editDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.editDocumentDirectoryToolStripMenuItem.Tag = "EDIT";
            this.editDocumentDirectoryToolStripMenuItem.Text = "Editar";
            this.editDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // indexDocumentDirectoryToolStripMenuItem
            // 
            this.indexDocumentDirectoryToolStripMenuItem.Name = "indexDocumentDirectoryToolStripMenuItem";
            this.indexDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.indexDocumentDirectoryToolStripMenuItem.Tag = "INDEX";
            this.indexDocumentDirectoryToolStripMenuItem.Text = "Indexar";
            this.indexDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // firstDocumentDirectoryToolStripSeparator
            // 
            this.firstDocumentDirectoryToolStripSeparator.Name = "firstDocumentDirectoryToolStripSeparator";
            this.firstDocumentDirectoryToolStripSeparator.Size = new System.Drawing.Size(93, 6);
            // 
            // cleanDocumentDirectoryToolStripMenuItem
            // 
            this.cleanDocumentDirectoryToolStripMenuItem.Name = "cleanDocumentDirectoryToolStripMenuItem";
            this.cleanDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.cleanDocumentDirectoryToolStripMenuItem.Tag = "CLEAN";
            this.cleanDocumentDirectoryToolStripMenuItem.Text = "Limpar";
            this.cleanDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // secondDocumentDirectoryToolStripSeparator
            // 
            this.secondDocumentDirectoryToolStripSeparator.Name = "secondDocumentDirectoryToolStripSeparator";
            this.secondDocumentDirectoryToolStripSeparator.Size = new System.Drawing.Size(93, 6);
            // 
            // removeDocumentDirectoryToolStripMenuItem
            // 
            this.removeDocumentDirectoryToolStripMenuItem.Name = "removeDocumentDirectoryToolStripMenuItem";
            this.removeDocumentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(96, 22);
            this.removeDocumentDirectoryToolStripMenuItem.Tag = "REMOVE";
            this.removeDocumentDirectoryToolStripMenuItem.Text = "Remover";
            this.removeDocumentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // documentDirectoryDataGridView
            // 
            this.documentDirectoryDataGridView.AllowUserToAddRows = false;
            this.documentDirectoryDataGridView.AllowUserToDeleteRows = false;
            this.documentDirectoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.documentDirectoryDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.positionDataGridViewTextBoxColumn,
            this.labelDataGridViewTextBoxColumn,
            this.pathDataGridViewTextBoxColumn});
            this.documentDirectoryDataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this.documentDirectoryDataGridView.Location = new System.Drawing.Point(10, 10);
            this.documentDirectoryDataGridView.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.documentDirectoryDataGridView.MultiSelect = false;
            this.documentDirectoryDataGridView.Name = "documentDirectoryDataGridView";
            this.documentDirectoryDataGridView.ReadOnly = true;
            this.documentDirectoryDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.documentDirectoryDataGridView.Size = new System.Drawing.Size(764, 200);
            this.documentDirectoryDataGridView.TabIndex = 0;
            this.documentDirectoryDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.documentDirectoryDataGridView_MouseClick);
            // 
            // positionDataGridViewTextBoxColumn
            // 
            this.positionDataGridViewTextBoxColumn.DataPropertyName = "Position";
            this.positionDataGridViewTextBoxColumn.HeaderText = "Posição";
            this.positionDataGridViewTextBoxColumn.Name = "positionDataGridViewTextBoxColumn";
            this.positionDataGridViewTextBoxColumn.ReadOnly = true;
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
            this.newDocumentDirectoryButton.Location = new System.Drawing.Point(10, 226);
            this.newDocumentDirectoryButton.Name = "newDocumentDirectoryButton";
            this.newDocumentDirectoryButton.Size = new System.Drawing.Size(300, 48);
            this.newDocumentDirectoryButton.TabIndex = 1;
            this.newDocumentDirectoryButton.Text = "Novo Diretório de Documentos";
            this.newDocumentDirectoryButton.UseVisualStyleBackColor = true;
            this.newDocumentDirectoryButton.Click += new System.EventHandler(this.newDocumentDirectoryButton_Click);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn positionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn labelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn pathDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripMenuItem cleanDocumentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator secondDocumentDirectoryToolStripSeparator;
    }
}