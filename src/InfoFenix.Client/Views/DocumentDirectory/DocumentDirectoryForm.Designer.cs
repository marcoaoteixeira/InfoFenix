namespace InfoFenix.Client.Views.DocumentDirectory {
    partial class DocumentDirectoryForm {
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
            this.separatorLabel = new System.Windows.Forms.Label();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.saveAndCloseButton = new System.Windows.Forms.Button();
            this.documentDirectoryPanel = new System.Windows.Forms.Panel();
            this.indexDocumentDirectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.watchDocumentDirectoryCheckBox = new System.Windows.Forms.CheckBox();
            this.selectDocumentDirectoryPathButton = new System.Windows.Forms.Button();
            this.documentDirectoryPathTextBox = new System.Windows.Forms.TextBox();
            this.documentDirectoryPathLabel = new System.Windows.Forms.Label();
            this.documentDirectoryLabelTextBox = new System.Windows.Forms.TextBox();
            this.documentDirectoryLabelLabel = new System.Windows.Forms.Label();
            this.saveUpdateDocumentsCheckBox = new System.Windows.Forms.CheckBox();
            this.actionPanel.SuspendLayout();
            this.documentDirectoryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // separatorLabel
            // 
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.separatorLabel.Location = new System.Drawing.Point(0, 194);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(584, 2);
            this.separatorLabel.TabIndex = 6;
            // 
            // actionPanel
            // 
            this.actionPanel.BackColor = System.Drawing.SystemColors.Window;
            this.actionPanel.Controls.Add(this.saveAndCloseButton);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actionPanel.Location = new System.Drawing.Point(0, 196);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Padding = new System.Windows.Forms.Padding(20);
            this.actionPanel.Size = new System.Drawing.Size(584, 80);
            this.actionPanel.TabIndex = 7;
            // 
            // saveAndCloseButton
            // 
            this.saveAndCloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveAndCloseButton.Location = new System.Drawing.Point(391, 20);
            this.saveAndCloseButton.Name = "saveAndCloseButton";
            this.saveAndCloseButton.Size = new System.Drawing.Size(170, 40);
            this.saveAndCloseButton.TabIndex = 0;
            this.saveAndCloseButton.Text = "Salvar e Fechar";
            this.saveAndCloseButton.UseVisualStyleBackColor = true;
            this.saveAndCloseButton.Click += new System.EventHandler(this.saveAndCloseButton_Click);
            // 
            // documentDirectoryPanel
            // 
            this.documentDirectoryPanel.Controls.Add(this.saveUpdateDocumentsCheckBox);
            this.documentDirectoryPanel.Controls.Add(this.indexDocumentDirectoryCheckBox);
            this.documentDirectoryPanel.Controls.Add(this.watchDocumentDirectoryCheckBox);
            this.documentDirectoryPanel.Controls.Add(this.selectDocumentDirectoryPathButton);
            this.documentDirectoryPanel.Controls.Add(this.documentDirectoryPathTextBox);
            this.documentDirectoryPanel.Controls.Add(this.documentDirectoryPathLabel);
            this.documentDirectoryPanel.Controls.Add(this.documentDirectoryLabelTextBox);
            this.documentDirectoryPanel.Controls.Add(this.documentDirectoryLabelLabel);
            this.documentDirectoryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentDirectoryPanel.Location = new System.Drawing.Point(0, 0);
            this.documentDirectoryPanel.Name = "documentDirectoryPanel";
            this.documentDirectoryPanel.Padding = new System.Windows.Forms.Padding(10);
            this.documentDirectoryPanel.Size = new System.Drawing.Size(584, 196);
            this.documentDirectoryPanel.TabIndex = 8;
            // 
            // indexDocumentDirectoryCheckBox
            // 
            this.indexDocumentDirectoryCheckBox.AutoSize = true;
            this.indexDocumentDirectoryCheckBox.Checked = true;
            this.indexDocumentDirectoryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.indexDocumentDirectoryCheckBox.Location = new System.Drawing.Point(17, 157);
            this.indexDocumentDirectoryCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.indexDocumentDirectoryCheckBox.Name = "indexDocumentDirectoryCheckBox";
            this.indexDocumentDirectoryCheckBox.Size = new System.Drawing.Size(82, 23);
            this.indexDocumentDirectoryCheckBox.TabIndex = 12;
            this.indexDocumentDirectoryCheckBox.Text = "Indexar";
            this.indexDocumentDirectoryCheckBox.UseVisualStyleBackColor = true;
            this.indexDocumentDirectoryCheckBox.CheckStateChanged += new System.EventHandler(this.indexDocumentDirectoryCheckBox_CheckStateChanged);
            // 
            // watchDocumentDirectoryCheckBox
            // 
            this.watchDocumentDirectoryCheckBox.AutoSize = true;
            this.watchDocumentDirectoryCheckBox.Checked = true;
            this.watchDocumentDirectoryCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.watchDocumentDirectoryCheckBox.Location = new System.Drawing.Point(105, 157);
            this.watchDocumentDirectoryCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.watchDocumentDirectoryCheckBox.Name = "watchDocumentDirectoryCheckBox";
            this.watchDocumentDirectoryCheckBox.Size = new System.Drawing.Size(92, 23);
            this.watchDocumentDirectoryCheckBox.TabIndex = 11;
            this.watchDocumentDirectoryCheckBox.Text = "Observar";
            this.watchDocumentDirectoryCheckBox.UseVisualStyleBackColor = true;
            this.watchDocumentDirectoryCheckBox.CheckStateChanged += new System.EventHandler(this.watchDocumentDirectoryCheckBox_CheckStateChanged);
            // 
            // selectDocumentDirectoryPathButton
            // 
            this.selectDocumentDirectoryPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectDocumentDirectoryPathButton.Location = new System.Drawing.Point(531, 114);
            this.selectDocumentDirectoryPathButton.Name = "selectDocumentDirectoryPathButton";
            this.selectDocumentDirectoryPathButton.Size = new System.Drawing.Size(40, 27);
            this.selectDocumentDirectoryPathButton.TabIndex = 10;
            this.selectDocumentDirectoryPathButton.Text = "...";
            this.selectDocumentDirectoryPathButton.UseVisualStyleBackColor = true;
            this.selectDocumentDirectoryPathButton.Click += new System.EventHandler(this.selectDocumentDirectoryPathButton_Click);
            // 
            // documentDirectoryPathTextBox
            // 
            this.documentDirectoryPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentDirectoryPathTextBox.Location = new System.Drawing.Point(17, 114);
            this.documentDirectoryPathTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.documentDirectoryPathTextBox.Name = "documentDirectoryPathTextBox";
            this.documentDirectoryPathTextBox.Size = new System.Drawing.Size(508, 27);
            this.documentDirectoryPathTextBox.TabIndex = 9;
            this.documentDirectoryPathTextBox.TextChanged += new System.EventHandler(this.documentDirectoryPathTextBox_TextChanged);
            this.documentDirectoryPathTextBox.Leave += new System.EventHandler(this.documentDirectoryPathTextBox_Leave);
            // 
            // documentDirectoryPathLabel
            // 
            this.documentDirectoryPathLabel.AutoSize = true;
            this.documentDirectoryPathLabel.Location = new System.Drawing.Point(13, 82);
            this.documentDirectoryPathLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.documentDirectoryPathLabel.Name = "documentDirectoryPathLabel";
            this.documentDirectoryPathLabel.Size = new System.Drawing.Size(273, 19);
            this.documentDirectoryPathLabel.TabIndex = 8;
            this.documentDirectoryPathLabel.Text = "Caminho do diretório de documentos";
            // 
            // documentDirectoryLabelTextBox
            // 
            this.documentDirectoryLabelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.documentDirectoryLabelTextBox.Location = new System.Drawing.Point(17, 42);
            this.documentDirectoryLabelTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.documentDirectoryLabelTextBox.Name = "documentDirectoryLabelTextBox";
            this.documentDirectoryLabelTextBox.Size = new System.Drawing.Size(554, 27);
            this.documentDirectoryLabelTextBox.TabIndex = 7;
            this.documentDirectoryLabelTextBox.TextChanged += new System.EventHandler(this.documentDirectoryLabelTextBox_TextChanged);
            this.documentDirectoryLabelTextBox.Leave += new System.EventHandler(this.documentDirectoryLabelTextBox_Leave);
            // 
            // documentDirectoryLabelLabel
            // 
            this.documentDirectoryLabelLabel.AutoSize = true;
            this.documentDirectoryLabelLabel.Location = new System.Drawing.Point(13, 10);
            this.documentDirectoryLabelLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.documentDirectoryLabelLabel.Name = "documentDirectoryLabelLabel";
            this.documentDirectoryLabelLabel.Size = new System.Drawing.Size(55, 19);
            this.documentDirectoryLabelLabel.TabIndex = 6;
            this.documentDirectoryLabelLabel.Text = "Rótulo";
            // 
            // saveUpdateDocumentsCheckBox
            // 
            this.saveUpdateDocumentsCheckBox.AutoSize = true;
            this.saveUpdateDocumentsCheckBox.Checked = true;
            this.saveUpdateDocumentsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveUpdateDocumentsCheckBox.Location = new System.Drawing.Point(203, 157);
            this.saveUpdateDocumentsCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.saveUpdateDocumentsCheckBox.Name = "saveUpdateDocumentsCheckBox";
            this.saveUpdateDocumentsCheckBox.Size = new System.Drawing.Size(236, 23);
            this.saveUpdateDocumentsCheckBox.TabIndex = 13;
            this.saveUpdateDocumentsCheckBox.Text = "Gravar/Atualizar Documentos";
            this.saveUpdateDocumentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // DocumentDirectoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 276);
            this.Controls.Add(this.separatorLabel);
            this.Controls.Add(this.documentDirectoryPanel);
            this.Controls.Add(this.actionPanel);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DocumentDirectoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.DocumentDirectoryForm_Load);
            this.actionPanel.ResumeLayout(false);
            this.documentDirectoryPanel.ResumeLayout(false);
            this.documentDirectoryPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label separatorLabel;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.Button saveAndCloseButton;
        private System.Windows.Forms.Panel documentDirectoryPanel;
        private System.Windows.Forms.Button selectDocumentDirectoryPathButton;
        private System.Windows.Forms.TextBox documentDirectoryPathTextBox;
        private System.Windows.Forms.Label documentDirectoryPathLabel;
        private System.Windows.Forms.TextBox documentDirectoryLabelTextBox;
        private System.Windows.Forms.Label documentDirectoryLabelLabel;
        private System.Windows.Forms.CheckBox indexDocumentDirectoryCheckBox;
        private System.Windows.Forms.CheckBox watchDocumentDirectoryCheckBox;
        private System.Windows.Forms.CheckBox saveUpdateDocumentsCheckBox;
    }
}