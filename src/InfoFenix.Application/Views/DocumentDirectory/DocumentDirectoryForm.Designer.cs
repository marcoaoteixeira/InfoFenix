namespace InfoFenix.Application.Views.DocumentDirectory {
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
            this.positionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.positionLabel = new System.Windows.Forms.Label();
            this.selectPathButton = new System.Windows.Forms.Button();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.pathLabel = new System.Windows.Forms.Label();
            this.labelTextBox = new System.Windows.Forms.TextBox();
            this.labelLabel = new System.Windows.Forms.Label();
            this.saveDocumentDirectoryDocumentsCheckBox = new System.Windows.Forms.CheckBox();
            this.actionPanel.SuspendLayout();
            this.documentDirectoryPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // separatorLabel
            // 
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.separatorLabel.Location = new System.Drawing.Point(0, 229);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(584, 2);
            this.separatorLabel.TabIndex = 6;
            // 
            // actionPanel
            // 
            this.actionPanel.BackColor = System.Drawing.SystemColors.Window;
            this.actionPanel.Controls.Add(this.saveAndCloseButton);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actionPanel.Location = new System.Drawing.Point(0, 231);
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
            this.saveAndCloseButton.TabIndex = 8;
            this.saveAndCloseButton.Text = "Salvar e Fechar";
            this.saveAndCloseButton.UseVisualStyleBackColor = true;
            this.saveAndCloseButton.Click += new System.EventHandler(this.saveAndCloseButton_Click);
            // 
            // documentDirectoryPanel
            // 
            this.documentDirectoryPanel.Controls.Add(this.saveDocumentDirectoryDocumentsCheckBox);
            this.documentDirectoryPanel.Controls.Add(this.positionNumericUpDown);
            this.documentDirectoryPanel.Controls.Add(this.positionLabel);
            this.documentDirectoryPanel.Controls.Add(this.selectPathButton);
            this.documentDirectoryPanel.Controls.Add(this.pathTextBox);
            this.documentDirectoryPanel.Controls.Add(this.pathLabel);
            this.documentDirectoryPanel.Controls.Add(this.labelTextBox);
            this.documentDirectoryPanel.Controls.Add(this.labelLabel);
            this.documentDirectoryPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentDirectoryPanel.Location = new System.Drawing.Point(0, 0);
            this.documentDirectoryPanel.Name = "documentDirectoryPanel";
            this.documentDirectoryPanel.Padding = new System.Windows.Forms.Padding(10);
            this.documentDirectoryPanel.Size = new System.Drawing.Size(584, 231);
            this.documentDirectoryPanel.TabIndex = 8;
            // 
            // positionNumericUpDown
            // 
            this.positionNumericUpDown.Location = new System.Drawing.Point(17, 186);
            this.positionNumericUpDown.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.positionNumericUpDown.Name = "positionNumericUpDown";
            this.positionNumericUpDown.Size = new System.Drawing.Size(120, 27);
            this.positionNumericUpDown.TabIndex = 6;
            this.positionNumericUpDown.ValueChanged += new System.EventHandler(this.positionNumericUpDown_ValueChanged);
            // 
            // positionLabel
            // 
            this.positionLabel.AutoSize = true;
            this.positionLabel.Location = new System.Drawing.Point(13, 154);
            this.positionLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.positionLabel.Name = "positionLabel";
            this.positionLabel.Size = new System.Drawing.Size(62, 19);
            this.positionLabel.TabIndex = 5;
            this.positionLabel.Text = "Posição";
            // 
            // selectPathButton
            // 
            this.selectPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectPathButton.Location = new System.Drawing.Point(531, 114);
            this.selectPathButton.Name = "selectPathButton";
            this.selectPathButton.Size = new System.Drawing.Size(40, 27);
            this.selectPathButton.TabIndex = 4;
            this.selectPathButton.Text = "...";
            this.selectPathButton.UseVisualStyleBackColor = true;
            this.selectPathButton.Click += new System.EventHandler(this.selectPathButton_Click);
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.Location = new System.Drawing.Point(17, 114);
            this.pathTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.Size = new System.Drawing.Size(508, 27);
            this.pathTextBox.TabIndex = 3;
            this.pathTextBox.TextChanged += new System.EventHandler(this.pathTextBox_TextChanged);
            this.pathTextBox.Leave += new System.EventHandler(this.pathTextBox_Leave);
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Location = new System.Drawing.Point(13, 82);
            this.pathLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(273, 19);
            this.pathLabel.TabIndex = 2;
            this.pathLabel.Text = "Caminho do diretório de documentos";
            // 
            // labelTextBox
            // 
            this.labelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTextBox.Location = new System.Drawing.Point(17, 42);
            this.labelTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(554, 27);
            this.labelTextBox.TabIndex = 1;
            this.labelTextBox.TextChanged += new System.EventHandler(this.labelTextBox_TextChanged);
            this.labelTextBox.Leave += new System.EventHandler(this.labelTextBox_Leave);
            // 
            // labelLabel
            // 
            this.labelLabel.AutoSize = true;
            this.labelLabel.Location = new System.Drawing.Point(13, 10);
            this.labelLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new System.Drawing.Size(55, 19);
            this.labelLabel.TabIndex = 0;
            this.labelLabel.Text = "Rótulo";
            // 
            // saveDocumentDirectoryDocumentsCheckBox
            // 
            this.saveDocumentDirectoryDocumentsCheckBox.AutoSize = true;
            this.saveDocumentDirectoryDocumentsCheckBox.Checked = true;
            this.saveDocumentDirectoryDocumentsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveDocumentDirectoryDocumentsCheckBox.Location = new System.Drawing.Point(345, 187);
            this.saveDocumentDirectoryDocumentsCheckBox.Name = "saveDocumentDirectoryDocumentsCheckBox";
            this.saveDocumentDirectoryDocumentsCheckBox.Size = new System.Drawing.Size(226, 23);
            this.saveDocumentDirectoryDocumentsCheckBox.TabIndex = 7;
            this.saveDocumentDirectoryDocumentsCheckBox.Text = "Salvar documentos na pasta";
            this.saveDocumentDirectoryDocumentsCheckBox.UseVisualStyleBackColor = true;
            // 
            // DocumentDirectoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(584, 311);
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
            ((System.ComponentModel.ISupportInitialize)(this.positionNumericUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label separatorLabel;
        private System.Windows.Forms.Panel actionPanel;
        private System.Windows.Forms.Button saveAndCloseButton;
        private System.Windows.Forms.Panel documentDirectoryPanel;
        private System.Windows.Forms.Button selectPathButton;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.TextBox labelTextBox;
        private System.Windows.Forms.Label labelLabel;
        private System.Windows.Forms.NumericUpDown positionNumericUpDown;
        private System.Windows.Forms.Label positionLabel;
        private System.Windows.Forms.CheckBox saveDocumentDirectoryDocumentsCheckBox;
    }
}