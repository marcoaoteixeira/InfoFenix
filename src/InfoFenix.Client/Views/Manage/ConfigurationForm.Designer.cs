﻿namespace InfoFenix.Client.Views.Manage {
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
            this.backupTabPage = new System.Windows.Forms.TabPage();
            this.performDatabaseRestoreButton = new System.Windows.Forms.Button();
            this.performDatabaseBackupButton = new System.Windows.Forms.Button();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.contentPanel.SuspendLayout();
            this.actionPanel.SuspendLayout();
            this.configurationTabControl.SuspendLayout();
            this.backupTabPage.SuspendLayout();
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
            this.configurationTabControl.Controls.Add(this.backupTabPage);
            this.configurationTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.configurationTabControl.Location = new System.Drawing.Point(10, 10);
            this.configurationTabControl.Name = "configurationTabControl";
            this.configurationTabControl.SelectedIndex = 0;
            this.configurationTabControl.Size = new System.Drawing.Size(464, 161);
            this.configurationTabControl.TabIndex = 0;
            // 
            // backupTabPage
            // 
            this.backupTabPage.Controls.Add(this.performDatabaseRestoreButton);
            this.backupTabPage.Controls.Add(this.performDatabaseBackupButton);
            this.backupTabPage.Location = new System.Drawing.Point(4, 28);
            this.backupTabPage.Name = "backupTabPage";
            this.backupTabPage.Padding = new System.Windows.Forms.Padding(10);
            this.backupTabPage.Size = new System.Drawing.Size(456, 129);
            this.backupTabPage.TabIndex = 1;
            this.backupTabPage.Text = "Backup e Restauração";
            this.backupTabPage.UseVisualStyleBackColor = true;
            // 
            // performDatabaseRestoreButton
            // 
            this.performDatabaseRestoreButton.Image = global::InfoFenix.Client.Properties.Resources.database_restore_32x32;
            this.performDatabaseRestoreButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.performDatabaseRestoreButton.Location = new System.Drawing.Point(13, 68);
            this.performDatabaseRestoreButton.Name = "performDatabaseRestoreButton";
            this.performDatabaseRestoreButton.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.performDatabaseRestoreButton.Size = new System.Drawing.Size(430, 48);
            this.performDatabaseRestoreButton.TabIndex = 1;
            this.performDatabaseRestoreButton.Text = "Executar &Restauração da Base de Dados";
            this.performDatabaseRestoreButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.performDatabaseRestoreButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.performDatabaseRestoreButton.UseVisualStyleBackColor = true;
            this.performDatabaseRestoreButton.Click += new System.EventHandler(this.performDatabaseRestoreButton_Click);
            // 
            // performDatabaseBackupButton
            // 
            this.performDatabaseBackupButton.Image = global::InfoFenix.Client.Properties.Resources.database_backup_32x32;
            this.performDatabaseBackupButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.performDatabaseBackupButton.Location = new System.Drawing.Point(13, 13);
            this.performDatabaseBackupButton.Name = "performDatabaseBackupButton";
            this.performDatabaseBackupButton.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.performDatabaseBackupButton.Size = new System.Drawing.Size(430, 48);
            this.performDatabaseBackupButton.TabIndex = 0;
            this.performDatabaseBackupButton.Text = "Executar &Backup da Base de Dados";
            this.performDatabaseBackupButton.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.performDatabaseBackupButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
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
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.contentPanel.ResumeLayout(false);
            this.actionPanel.ResumeLayout(false);
            this.configurationTabControl.ResumeLayout(false);
            this.backupTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl configurationTabControl;
        private System.Windows.Forms.TabPage backupTabPage;
        private System.Windows.Forms.Button performDatabaseRestoreButton;
        private System.Windows.Forms.Button performDatabaseBackupButton;
    }
}