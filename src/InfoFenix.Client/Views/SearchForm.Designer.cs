namespace InfoFenix.Client.Views {
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
            this.searchResultDataGridView = new System.Windows.Forms.DataGridView();
            this.labelDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.actualTotalDocumentsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalDocumentsFoundDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.searchTermPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchResultDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::InfoFenix.Client.Properties.Resources.search_128x128;
            // 
            // titleLabel
            // 
            this.titleLabel.Text = "Pesquisar";
            // 
            // subTitleLabel
            // 
            this.subTitleLabel.Text = "Pesquise por todos os diretórios de documentos que foram adicionados a base de da" +
    "dos.";
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.searchResultDataGridView);
            this.contentPanel.Controls.Add(this.searchTermPanel);
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0);
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
            this.searchTermPanel.Size = new System.Drawing.Size(784, 60);
            this.searchTermPanel.TabIndex = 0;
            // 
            // searchTermTextBox
            // 
            this.searchTermTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTermTextBox.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchTermTextBox.Location = new System.Drawing.Point(131, 15);
            this.searchTermTextBox.Name = "searchTermTextBox";
            this.searchTermTextBox.Size = new System.Drawing.Size(532, 30);
            this.searchTermTextBox.TabIndex = 2;
            // 
            // executeSearchButton
            // 
            this.executeSearchButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.executeSearchButton.Location = new System.Drawing.Point(674, 10);
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
            // searchResultDataGridView
            // 
            this.searchResultDataGridView.AllowUserToAddRows = false;
            this.searchResultDataGridView.AllowUserToDeleteRows = false;
            this.searchResultDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.searchResultDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.labelDataGridViewTextBoxColumn,
            this.actualTotalDocumentsDataGridViewTextBoxColumn,
            this.totalDocumentsFoundDataGridViewTextBoxColumn});
            this.searchResultDataGridView.Location = new System.Drawing.Point(10, 73);
            this.searchResultDataGridView.Margin = new System.Windows.Forms.Padding(10);
            this.searchResultDataGridView.MultiSelect = false;
            this.searchResultDataGridView.Name = "searchResultDataGridView";
            this.searchResultDataGridView.ReadOnly = true;
            this.searchResultDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.searchResultDataGridView.Size = new System.Drawing.Size(764, 297);
            this.searchResultDataGridView.TabIndex = 1;
            this.searchResultDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.searchResultDataGridView_CellDoubleClick);
            // 
            // labelDataGridViewTextBoxColumn
            // 
            this.labelDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.labelDataGridViewTextBoxColumn.HeaderText = "Rótulo";
            this.labelDataGridViewTextBoxColumn.Name = "labelDataGridViewTextBoxColumn";
            this.labelDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // actualTotalDocumentsDataGridViewTextBoxColumn
            // 
            this.actualTotalDocumentsDataGridViewTextBoxColumn.HeaderText = "Total de documentos";
            this.actualTotalDocumentsDataGridViewTextBoxColumn.Name = "actualTotalDocumentsDataGridViewTextBoxColumn";
            this.actualTotalDocumentsDataGridViewTextBoxColumn.ReadOnly = true;
            this.actualTotalDocumentsDataGridViewTextBoxColumn.Width = 200;
            // 
            // totalDocumentsFoundDataGridViewTextBoxColumn
            // 
            this.totalDocumentsFoundDataGridViewTextBoxColumn.HeaderText = "Documentos encontrados";
            this.totalDocumentsFoundDataGridViewTextBoxColumn.Name = "totalDocumentsFoundDataGridViewTextBoxColumn";
            this.totalDocumentsFoundDataGridViewTextBoxColumn.ReadOnly = true;
            this.totalDocumentsFoundDataGridViewTextBoxColumn.Width = 200;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Name = "SearchForm";
            this.Text = "Pesquisar";
            this.Load += new System.EventHandler(this.SearchForm_Load);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.searchTermPanel.ResumeLayout(false);
            this.searchTermPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.searchResultDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel searchTermPanel;
        private System.Windows.Forms.DataGridView searchResultDataGridView;
        private System.Windows.Forms.TextBox searchTermTextBox;
        private System.Windows.Forms.Button executeSearchButton;
        private System.Windows.Forms.Label searchByLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn labelDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn actualTotalDocumentsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalDocumentsFoundDataGridViewTextBoxColumn;
    }
}