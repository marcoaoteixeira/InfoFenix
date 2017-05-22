namespace InfoFenix.Client.Views {
    partial class SearchDocumentForm {
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
            this.queryTermPanel = new System.Windows.Forms.Panel();
            this.doSearchButton = new System.Windows.Forms.Button();
            this.queryTermTextBox = new System.Windows.Forms.TextBox();
            this.searchResultPanel = new System.Windows.Forms.Panel();
            this.searchResultDataGridView = new System.Windows.Forms.DataGridView();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.queryTermPanel.SuspendLayout();
            this.searchResultPanel.SuspendLayout();
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
            this.subTitleLabel.Text = "Utilize o campo abaixo para realizar a pesquisa de documentos. Dê um duplo clique" +
    " na linha de resultados para abrir a exibição de documentos.";
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.searchResultPanel);
            this.contentPanel.Controls.Add(this.queryTermPanel);
            this.contentPanel.Padding = new System.Windows.Forms.Padding(0);
            // 
            // queryTermPanel
            // 
            this.queryTermPanel.Controls.Add(this.doSearchButton);
            this.queryTermPanel.Controls.Add(this.queryTermTextBox);
            this.queryTermPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.queryTermPanel.Location = new System.Drawing.Point(0, 0);
            this.queryTermPanel.Name = "queryTermPanel";
            this.queryTermPanel.Padding = new System.Windows.Forms.Padding(10);
            this.queryTermPanel.Size = new System.Drawing.Size(784, 64);
            this.queryTermPanel.TabIndex = 0;
            // 
            // doSearchButton
            // 
            this.doSearchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doSearchButton.Location = new System.Drawing.Point(661, 13);
            this.doSearchButton.Name = "doSearchButton";
            this.doSearchButton.Size = new System.Drawing.Size(110, 36);
            this.doSearchButton.TabIndex = 1;
            this.doSearchButton.Text = "Pesquisar";
            this.doSearchButton.UseVisualStyleBackColor = true;
            // 
            // queryTermTextBox
            // 
            this.queryTermTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.queryTermTextBox.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.queryTermTextBox.Location = new System.Drawing.Point(13, 13);
            this.queryTermTextBox.Name = "queryTermTextBox";
            this.queryTermTextBox.Size = new System.Drawing.Size(642, 36);
            this.queryTermTextBox.TabIndex = 0;
            // 
            // searchResultPanel
            // 
            this.searchResultPanel.Controls.Add(this.searchResultDataGridView);
            this.searchResultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchResultPanel.Location = new System.Drawing.Point(0, 64);
            this.searchResultPanel.Name = "searchResultPanel";
            this.searchResultPanel.Padding = new System.Windows.Forms.Padding(10);
            this.searchResultPanel.Size = new System.Drawing.Size(784, 317);
            this.searchResultPanel.TabIndex = 1;
            // 
            // searchResultDataGridView
            // 
            this.searchResultDataGridView.AllowUserToAddRows = false;
            this.searchResultDataGridView.AllowUserToDeleteRows = false;
            this.searchResultDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.searchResultDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchResultDataGridView.Location = new System.Drawing.Point(10, 10);
            this.searchResultDataGridView.Name = "searchResultDataGridView";
            this.searchResultDataGridView.ReadOnly = true;
            this.searchResultDataGridView.Size = new System.Drawing.Size(764, 297);
            this.searchResultDataGridView.TabIndex = 0;
            // 
            // SearchDocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Name = "SearchDocumentForm";
            this.Text = "Pesquisar";
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.queryTermPanel.ResumeLayout(false);
            this.queryTermPanel.PerformLayout();
            this.searchResultPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.searchResultDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel searchResultPanel;
        private System.Windows.Forms.Panel queryTermPanel;
        private System.Windows.Forms.Button doSearchButton;
        private System.Windows.Forms.TextBox queryTermTextBox;
        private System.Windows.Forms.DataGridView searchResultDataGridView;
    }
}