namespace InfoFenix.Client.Views.Search {
    partial class SearchForm {
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
            this.searchTermPanel = new System.Windows.Forms.Panel();
            this.searchTermTextBox = new System.Windows.Forms.TextBox();
            this.executeSearchButton = new System.Windows.Forms.Button();
            this.searchByLabel = new System.Windows.Forms.Label();
            this.documentDirectoriesDataGridView = new System.Windows.Forms.DataGridView();
            this.labelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actualTotalDocumentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.foundTotalDocumentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.informationLabel = new System.Windows.Forms.Label();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.searchTermPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentDirectoriesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.Size = new System.Drawing.Size(634, 96);
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::InfoFenix.Client.Properties.Resources.search_128x128;
            // 
            // titleLabel
            // 
            this.titleLabel.Size = new System.Drawing.Size(538, 56);
            this.titleLabel.Text = "Pesquisar";
            // 
            // subTitleLabel
            // 
            this.subTitleLabel.Size = new System.Drawing.Size(538, 40);
            this.subTitleLabel.Text = "Pesquise por todos os diretórios de documentos que foram adicionados a base de da" +
    "dos.";
            // 
            // topSeparator
            // 
            this.topSeparator.Size = new System.Drawing.Size(634, 2);
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.documentDirectoriesDataGridView);
            this.contentPanel.Controls.Add(this.searchTermPanel);
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
            this.actionPanel.Controls.Add(this.informationLabel);
            this.actionPanel.Location = new System.Drawing.Point(0, 381);
            this.actionPanel.Size = new System.Drawing.Size(634, 80);
            this.actionPanel.Controls.SetChildIndex(this.closeButton, 0);
            this.actionPanel.Controls.SetChildIndex(this.informationLabel, 0);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(494, 20);
            this.closeButton.Margin = new System.Windows.Forms.Padding(23, 3, 3, 3);
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // searchTermPanel
            // 
            this.searchTermPanel.Controls.Add(this.searchTermTextBox);
            this.searchTermPanel.Controls.Add(this.executeSearchButton);
            this.searchTermPanel.Controls.Add(this.searchByLabel);
            this.searchTermPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchTermPanel.Location = new System.Drawing.Point(0, 0);
            this.searchTermPanel.Name = "searchTermPanel";
            this.searchTermPanel.Padding = new System.Windows.Forms.Padding(10);
            this.searchTermPanel.Size = new System.Drawing.Size(634, 60);
            this.searchTermPanel.TabIndex = 0;
            // 
            // searchTermTextBox
            // 
            this.searchTermTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTermTextBox.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTermTextBox.Location = new System.Drawing.Point(131, 15);
            this.searchTermTextBox.Name = "searchTermTextBox";
            this.searchTermTextBox.Size = new System.Drawing.Size(382, 30);
            this.searchTermTextBox.TabIndex = 2;
            this.searchTermTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTermTextBox_KeyDown);
            // 
            // executeSearchButton
            // 
            this.executeSearchButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.executeSearchButton.Location = new System.Drawing.Point(524, 10);
            this.executeSearchButton.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.executeSearchButton.Name = "executeSearchButton";
            this.executeSearchButton.Size = new System.Drawing.Size(100, 40);
            this.executeSearchButton.TabIndex = 1;
            this.executeSearchButton.Text = "Pesquisar";
            this.executeSearchButton.UseVisualStyleBackColor = true;
            this.executeSearchButton.Click += new System.EventHandler(this.executeSearchButton_Click);
            // 
            // searchByLabel
            // 
            this.searchByLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.searchByLabel.Location = new System.Drawing.Point(10, 10);
            this.searchByLabel.Name = "searchByLabel";
            this.searchByLabel.Size = new System.Drawing.Size(115, 40);
            this.searchByLabel.TabIndex = 0;
            this.searchByLabel.Text = "Pesquisar por:";
            this.searchByLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // documentDirectoriesDataGridView
            // 
            this.documentDirectoriesDataGridView.AllowUserToAddRows = false;
            this.documentDirectoriesDataGridView.AllowUserToDeleteRows = false;
            this.documentDirectoriesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentDirectoriesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.documentDirectoriesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.labelDataGridViewTextBoxColumn,
            this.actualTotalDocumentsDataGridViewTextBoxColumn,
            this.foundTotalDocumentsDataGridViewTextBoxColumn});
            this.documentDirectoriesDataGridView.Location = new System.Drawing.Point(10, 73);
            this.documentDirectoriesDataGridView.Margin = new System.Windows.Forms.Padding(10);
            this.documentDirectoriesDataGridView.MultiSelect = false;
            this.documentDirectoriesDataGridView.Name = "documentDirectoriesDataGridView";
            this.documentDirectoriesDataGridView.ReadOnly = true;
            this.documentDirectoriesDataGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.documentDirectoriesDataGridView.RowTemplate.Height = 32;
            this.documentDirectoriesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.documentDirectoriesDataGridView.Size = new System.Drawing.Size(614, 197);
            this.documentDirectoriesDataGridView.TabIndex = 1;
            this.documentDirectoriesDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.searchResultDataGridView_CellDoubleClick);
            // 
            // labelDataGridViewTextBoxColumn
            // 
            this.labelDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.labelDataGridViewTextBoxColumn.DataPropertyName = "Label";
            this.labelDataGridViewTextBoxColumn.HeaderText = "Rótulo";
            this.labelDataGridViewTextBoxColumn.Name = "labelDataGridViewTextBoxColumn";
            this.labelDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // actualTotalDocumentsDataGridViewTextBoxColumn
            // 
            this.actualTotalDocumentsDataGridViewTextBoxColumn.DataPropertyName = "TotalDocuments";
            this.actualTotalDocumentsDataGridViewTextBoxColumn.HeaderText = "Total de documentos";
            this.actualTotalDocumentsDataGridViewTextBoxColumn.Name = "actualTotalDocumentsDataGridViewTextBoxColumn";
            this.actualTotalDocumentsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // foundTotalDocumentsDataGridViewTextBoxColumn
            // 
            this.foundTotalDocumentsDataGridViewTextBoxColumn.DataPropertyName = "TotalDocumentsFound";
            this.foundTotalDocumentsDataGridViewTextBoxColumn.HeaderText = "Documentos encontrados";
            this.foundTotalDocumentsDataGridViewTextBoxColumn.Name = "foundTotalDocumentsDataGridViewTextBoxColumn";
            this.foundTotalDocumentsDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // informationLabel
            // 
            this.informationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.informationLabel.Location = new System.Drawing.Point(23, 20);
            this.informationLabel.Name = "informationLabel";
            this.informationLabel.Size = new System.Drawing.Size(445, 40);
            this.informationLabel.TabIndex = 1;
            this.informationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 461);
            this.Name = "SearchForm";
            this.Text = "Pesquisar";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.searchTermPanel.ResumeLayout(false);
            this.searchTermPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentDirectoriesDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel searchTermPanel;
        private System.Windows.Forms.DataGridView documentDirectoriesDataGridView;
        private System.Windows.Forms.TextBox searchTermTextBox;
        private System.Windows.Forms.Button executeSearchButton;
        private System.Windows.Forms.Label searchByLabel;
        private System.Windows.Forms.Label informationLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn labelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actualTotalDocumentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn foundTotalDocumentsDataGridViewTextBoxColumn;
    }
}