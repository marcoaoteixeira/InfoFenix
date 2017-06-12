namespace InfoFenix.Client.Views.UserControls {
    partial class TaskItemUserControl {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskItemUserControl));
            this.mainImageList = new System.Windows.Forms.ImageList(this.components);
            this.mainLabel = new System.Windows.Forms.Label();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // mainImageList
            // 
            this.mainImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainImageList.ImageStream")));
            this.mainImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.mainImageList.Images.SetKeyName(0, "none_24x24.png");
            this.mainImageList.Images.SetKeyName(1, "inprogress_24x24.png");
            this.mainImageList.Images.SetKeyName(2, "complete_24x24.png");
            this.mainImageList.Images.SetKeyName(3, "error_24x24.png");
            // 
            // mainLabel
            // 
            this.mainLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainLabel.AutoEllipsis = true;
            this.mainLabel.Location = new System.Drawing.Point(35, 0);
            this.mainLabel.Margin = new System.Windows.Forms.Padding(0);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size(265, 32);
            this.mainLabel.TabIndex = 1;
            this.mainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mainPictureBox.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Padding = new System.Windows.Forms.Padding(4);
            this.mainPictureBox.Size = new System.Drawing.Size(32, 32);
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            // 
            // TaskItemUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.mainLabel);
            this.Controls.Add(this.mainPictureBox);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(150, 32);
            this.Name = "TaskItemUserControl";
            this.Size = new System.Drawing.Size(300, 32);
            this.Load += new System.EventHandler(this.TaskItemUserControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList mainImageList;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.Label mainLabel;
    }
}
