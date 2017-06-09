namespace InfoFenix.Client.Views {
    partial class ConfigurationForm {
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
            this.configurationTabControl = new System.Windows.Forms.TabControl();
            this.remoteConfigurationTabPage = new System.Windows.Forms.TabPage();
            this.selectRemoteDatabaseDirectoryPathButton = new System.Windows.Forms.Button();
            this.remoteDatabaseDirectoryPathTextBox = new System.Windows.Forms.TextBox();
            this.remoteDatabaseDirectoryPathLabel = new System.Windows.Forms.Label();
            this.useRemoteDatabaseCheckBox = new System.Windows.Forms.CheckBox();
            this.backupDatabaseTabPage = new System.Windows.Forms.TabPage();
            this.performDatabaseBackupButton = new System.Windows.Forms.Button();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.configurationTabControl.SuspendLayout();
            this.remoteConfigurationTabPage.SuspendLayout();
            this.backupDatabaseTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.Size = new System.Drawing.Size(484, 96);
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Image = global::InfoFenix.Client.Properties.Resources.settings_128x128;
            // 
            // titleLabel
            // 
            this.titleLabel.Size = new System.Drawing.Size(388, 56);
            this.titleLabel.Text = "Configurações";
            // 
            // subTitleLabel
            // 
            this.subTitleLabel.Size = new System.Drawing.Size(388, 40);
            this.subTitleLabel.Text = "Nessa tela é possível configurar alguns aspectos do aplicativo, além da realizaçã" +
    "o de backup da base de dados.";
            // 
            // topSeparator
            // 
            this.topSeparator.Size = new System.Drawing.Size(484, 2);
            // 
            // contentPanel
            // 
            this.contentPanel.Controls.Add(this.configurationTabControl);
            this.contentPanel.Size = new System.Drawing.Size(484, 181);
            // 
            // bottomSeparator
            // 
            this.bottomSeparator.Location = new System.Drawing.Point(0, 279);
            this.bottomSeparator.Size = new System.Drawing.Size(484, 2);
            // 
            // actionPanel
            // 
            this.actionPanel.Location = new System.Drawing.Point(0, 281);
            this.actionPanel.Size = new System.Drawing.Size(484, 80);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(344, 20);
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // configurationTabControl
            // 
            this.configurationTabControl.Controls.Add(this.remoteConfigurationTabPage);
            this.configurationTabControl.Controls.Add(this.backupDatabaseTabPage);
            this.configurationTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configurationTabControl.Location = new System.Drawing.Point(10, 10);
            this.configurationTabControl.Name = "configurationTabControl";
            this.configurationTabControl.SelectedIndex = 0;
            this.configurationTabControl.Size = new System.Drawing.Size(464, 161);
            this.configurationTabControl.TabIndex = 0;
            // 
            // remoteConfigurationTabPage
            // 
            this.remoteConfigurationTabPage.Controls.Add(this.selectRemoteDatabaseDirectoryPathButton);
            this.remoteConfigurationTabPage.Controls.Add(this.remoteDatabaseDirectoryPathTextBox);
            this.remoteConfigurationTabPage.Controls.Add(this.remoteDatabaseDirectoryPathLabel);
            this.remoteConfigurationTabPage.Controls.Add(this.useRemoteDatabaseCheckBox);
            this.remoteConfigurationTabPage.Location = new System.Drawing.Point(4, 28);
            this.remoteConfigurationTabPage.Name = "remoteConfigurationTabPage";
            this.remoteConfigurationTabPage.Padding = new System.Windows.Forms.Padding(10);
            this.remoteConfigurationTabPage.Size = new System.Drawing.Size(456, 129);
            this.remoteConfigurationTabPage.TabIndex = 0;
            this.remoteConfigurationTabPage.Text = "Remoto";
            this.remoteConfigurationTabPage.UseVisualStyleBackColor = true;
            // 
            // selectRemoteDatabaseDirectoryPathButton
            // 
            this.selectRemoteDatabaseDirectoryPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.selectRemoteDatabaseDirectoryPathButton.Location = new System.Drawing.Point(408, 81);
            this.selectRemoteDatabaseDirectoryPathButton.Name = "selectRemoteDatabaseDirectoryPathButton";
            this.selectRemoteDatabaseDirectoryPathButton.Size = new System.Drawing.Size(35, 27);
            this.selectRemoteDatabaseDirectoryPathButton.TabIndex = 3;
            this.selectRemoteDatabaseDirectoryPathButton.Text = "...";
            this.selectRemoteDatabaseDirectoryPathButton.UseVisualStyleBackColor = true;
            this.selectRemoteDatabaseDirectoryPathButton.Click += new System.EventHandler(this.selectRemoteDatabaseDirectoryPathButton_Click);
            // 
            // remoteDatabaseDirectoryPathTextBox
            // 
            this.remoteDatabaseDirectoryPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.remoteDatabaseDirectoryPathTextBox.Location = new System.Drawing.Point(13, 81);
            this.remoteDatabaseDirectoryPathTextBox.Name = "remoteDatabaseDirectoryPathTextBox";
            this.remoteDatabaseDirectoryPathTextBox.Size = new System.Drawing.Size(389, 27);
            this.remoteDatabaseDirectoryPathTextBox.TabIndex = 2;
            this.remoteDatabaseDirectoryPathTextBox.TextChanged += new System.EventHandler(this.remoteDatabaseDirectoryPathTextBox_TextChanged);
            this.remoteDatabaseDirectoryPathTextBox.Leave += new System.EventHandler(this.remoteDatabaseDirectoryPathTextBox_Leave);
            // 
            // remoteDatabaseDirectoryPathLabel
            // 
            this.remoteDatabaseDirectoryPathLabel.AutoSize = true;
            this.remoteDatabaseDirectoryPathLabel.Location = new System.Drawing.Point(13, 49);
            this.remoteDatabaseDirectoryPathLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 10);
            this.remoteDatabaseDirectoryPathLabel.Name = "remoteDatabaseDirectoryPathLabel";
            this.remoteDatabaseDirectoryPathLabel.Size = new System.Drawing.Size(370, 19);
            this.remoteDatabaseDirectoryPathLabel.TabIndex = 1;
            this.remoteDatabaseDirectoryPathLabel.Text = "Caminho para o diretório da base de dados remota";
            // 
            // useRemoteDatabaseCheckBox
            // 
            this.useRemoteDatabaseCheckBox.AutoSize = true;
            this.useRemoteDatabaseCheckBox.Location = new System.Drawing.Point(13, 13);
            this.useRemoteDatabaseCheckBox.Margin = new System.Windows.Forms.Padding(3, 3, 3, 13);
            this.useRemoteDatabaseCheckBox.Name = "useRemoteDatabaseCheckBox";
            this.useRemoteDatabaseCheckBox.Size = new System.Drawing.Size(238, 23);
            this.useRemoteDatabaseCheckBox.TabIndex = 0;
            this.useRemoteDatabaseCheckBox.Text = "Utilizar base de dados remota";
            this.useRemoteDatabaseCheckBox.UseVisualStyleBackColor = true;
            this.useRemoteDatabaseCheckBox.CheckedChanged += new System.EventHandler(this.useRemoteDatabaseCheckBox_CheckedChanged);
            // 
            // backupDatabaseTabPage
            // 
            this.backupDatabaseTabPage.Controls.Add(this.performDatabaseBackupButton);
            this.backupDatabaseTabPage.Location = new System.Drawing.Point(4, 28);
            this.backupDatabaseTabPage.Name = "backupDatabaseTabPage";
            this.backupDatabaseTabPage.Padding = new System.Windows.Forms.Padding(10);
            this.backupDatabaseTabPage.Size = new System.Drawing.Size(456, 129);
            this.backupDatabaseTabPage.TabIndex = 1;
            this.backupDatabaseTabPage.Text = "Backup";
            this.backupDatabaseTabPage.UseVisualStyleBackColor = true;
            // 
            // performDatabaseBackupButton
            // 
            this.performDatabaseBackupButton.Location = new System.Drawing.Point(13, 13);
            this.performDatabaseBackupButton.Name = "performDatabaseBackupButton";
            this.performDatabaseBackupButton.Size = new System.Drawing.Size(300, 45);
            this.performDatabaseBackupButton.TabIndex = 0;
            this.performDatabaseBackupButton.Text = "Realizar backup da base de dados";
            this.performDatabaseBackupButton.UseVisualStyleBackColor = true;
            this.performDatabaseBackupButton.Click += new System.EventHandler(this.performDatabaseBackupButton_Click);
            // 
            // ConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ConfigurationForm";
            this.Text = "Configurações";
            this.Load += new System.EventHandler(this.ConfigurationForm_Load);
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.configurationTabControl.ResumeLayout(false);
            this.remoteConfigurationTabPage.ResumeLayout(false);
            this.remoteConfigurationTabPage.PerformLayout();
            this.backupDatabaseTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl configurationTabControl;
        private System.Windows.Forms.TabPage remoteConfigurationTabPage;
        private System.Windows.Forms.Button selectRemoteDatabaseDirectoryPathButton;
        private System.Windows.Forms.TextBox remoteDatabaseDirectoryPathTextBox;
        private System.Windows.Forms.Label remoteDatabaseDirectoryPathLabel;
        private System.Windows.Forms.CheckBox useRemoteDatabaseCheckBox;
        private System.Windows.Forms.TabPage backupDatabaseTabPage;
        private System.Windows.Forms.Button performDatabaseBackupButton;
    }
}