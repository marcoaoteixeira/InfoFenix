using System.ComponentModel;
using System.Windows.Forms;

namespace InfoFenix.Application.Views.Shared {

    public class LayoutForm : Form {

        #region Public Fields

        public Panel titlePanel;
        public PictureBox iconPictureBox;
        public Label titleLabel;
        public Label subTitleLabel;
        public Label topSeparator;
        public Panel contentPanel;
        public Label bottomSeparator;
        public Panel actionPanel;
        public Button closeButton;

        #endregion Public Fields

        #region Private Fields

        private IContainer components = null;

        #endregion Private Fields

        #region Public Properties

        private bool _showHeader = true;
        [Browsable(true)]
        public bool ShowHeader {
            get { return _showHeader; }
            set { InnerShowHeader(value); }
        }

        #endregion

        #region Public Constructors

        public LayoutForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Protected Override Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion Protected Override Methods

        #region Private Methods

        private void InnerShowHeader(bool show) {
            titlePanel.Visible = show;
            topSeparator.Visible = show;

            _showHeader = show;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.titlePanel = new System.Windows.Forms.Panel();
            this.subTitleLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.topSeparator = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.bottomSeparator = new System.Windows.Forms.Label();
            this.actionPanel = new System.Windows.Forms.Panel();
            this.closeButton = new System.Windows.Forms.Button();
            this.titlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.actionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.SystemColors.Window;
            this.titlePanel.Controls.Add(this.subTitleLabel);
            this.titlePanel.Controls.Add(this.titleLabel);
            this.titlePanel.Controls.Add(this.iconPictureBox);
            this.titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titlePanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titlePanel.Location = new System.Drawing.Point(0, 0);
            this.titlePanel.Margin = new System.Windows.Forms.Padding(4);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(784, 96);
            this.titlePanel.TabIndex = 0;
            // 
            // subTitleLabel
            // 
            this.subTitleLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.subTitleLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.subTitleLabel.Location = new System.Drawing.Point(96, 56);
            this.subTitleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.subTitleLabel.Name = "subTitleLabel";
            this.subTitleLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.subTitleLabel.Size = new System.Drawing.Size(688, 40);
            this.subTitleLabel.TabIndex = 2;
            this.subTitleLabel.Text = "###";
            this.subTitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // titleLabel
            // 
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(96, 0);
            this.titleLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.titleLabel.Size = new System.Drawing.Size(688, 56);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "###";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconPictureBox.Location = new System.Drawing.Point(0, 0);
            this.iconPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 12, 4);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Padding = new System.Windows.Forms.Padding(10);
            this.iconPictureBox.Size = new System.Drawing.Size(96, 96);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconPictureBox.TabIndex = 0;
            this.iconPictureBox.TabStop = false;
            // 
            // topSeparator
            // 
            this.topSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.topSeparator.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSeparator.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.topSeparator.Location = new System.Drawing.Point(0, 96);
            this.topSeparator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.topSeparator.Name = "topSeparator";
            this.topSeparator.Size = new System.Drawing.Size(784, 2);
            this.topSeparator.TabIndex = 1;
            // 
            // contentPanel
            // 
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contentPanel.Location = new System.Drawing.Point(0, 98);
            this.contentPanel.Margin = new System.Windows.Forms.Padding(4);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(10);
            this.contentPanel.Size = new System.Drawing.Size(784, 381);
            this.contentPanel.TabIndex = 2;
            // 
            // bottomSeparator
            // 
            this.bottomSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.bottomSeparator.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSeparator.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bottomSeparator.Location = new System.Drawing.Point(0, 479);
            this.bottomSeparator.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.bottomSeparator.Name = "bottomSeparator";
            this.bottomSeparator.Size = new System.Drawing.Size(784, 2);
            this.bottomSeparator.TabIndex = 3;
            // 
            // actionPanel
            // 
            this.actionPanel.BackColor = System.Drawing.SystemColors.Window;
            this.actionPanel.Controls.Add(this.closeButton);
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.actionPanel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionPanel.Location = new System.Drawing.Point(0, 481);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Padding = new System.Windows.Forms.Padding(20);
            this.actionPanel.Size = new System.Drawing.Size(784, 80);
            this.actionPanel.TabIndex = 4;
            // 
            // closeButton
            // 
            this.closeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.closeButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.closeButton.Location = new System.Drawing.Point(644, 20);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(120, 40);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "&Fechar";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // LayoutForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.bottomSeparator);
            this.Controls.Add(this.actionPanel);
            this.Controls.Add(this.topSeparator);
            this.Controls.Add(this.titlePanel);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LayoutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "###";
            this.titlePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.actionPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion Private Methods
    }
}