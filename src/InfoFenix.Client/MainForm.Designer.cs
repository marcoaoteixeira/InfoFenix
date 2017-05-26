namespace InfoFenix.Client {
    partial class MainForm {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent() {
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.documentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpInformationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.firstToolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.indexingInformationToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainMenuStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.searchToolStripMenuItem,
            this.documentDirectoryToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.mainMenuStrip.Size = new System.Drawing.Size(1112, 24);
            this.mainMenuStrip.TabIndex = 1;
            // 
            // searchToolStripMenuItem
            // 
            this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
            this.searchToolStripMenuItem.Size = new System.Drawing.Size(69, 20);
            this.searchToolStripMenuItem.Text = "Pesquisar";
            this.searchToolStripMenuItem.Click += new System.EventHandler(this.searchToolStripMenuItem_Click);
            // 
            // documentDirectoryToolStripMenuItem
            // 
            this.documentDirectoryToolStripMenuItem.Name = "documentDirectoryToolStripMenuItem";
            this.documentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(157, 20);
            this.documentDirectoryToolStripMenuItem.Text = "Diretórios de Documentos";
            this.documentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.documentDirectoryToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(96, 20);
            this.configurationToolStripMenuItem.Text = "Configurações";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpInformationToolStripMenuItem,
            this.firstToolStripSeparator,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.helpToolStripMenuItem.Text = "Ajuda";
            // 
            // helpInformationToolStripMenuItem
            // 
            this.helpInformationToolStripMenuItem.Name = "helpInformationToolStripMenuItem";
            this.helpInformationToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.helpInformationToolStripMenuItem.Text = "Informações";
            // 
            // firstToolStripSeparator
            // 
            this.firstToolStripSeparator.Name = "firstToolStripSeparator";
            this.firstToolStripSeparator.Size = new System.Drawing.Size(137, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.aboutToolStripMenuItem.Text = "Sobre";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.exitToolStripMenuItem.Text = "Sair";
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.indexingInformationToolStripStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 730);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.mainStatusStrip.Size = new System.Drawing.Size(1112, 22);
            this.mainStatusStrip.TabIndex = 2;
            // 
            // indexingInformationToolStripStatusLabel
            // 
            this.indexingInformationToolStripStatusLabel.Name = "indexingInformationToolStripStatusLabel";
            this.indexingInformationToolStripStatusLabel.Size = new System.Drawing.Size(156, 17);
            this.indexingInformationToolStripStatusLabel.Text = "INFORMACAO_INDEXACAO";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1112, 752);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenuStrip;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Info Fênix";
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem documentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpInformationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator firstToolStripSeparator;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel indexingInformationToolStripStatusLabel;
    }
}

