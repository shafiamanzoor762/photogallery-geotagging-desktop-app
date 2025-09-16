namespace PhotoGallery_Desktop
{
    partial class SecondScreen
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SecondScreen));
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.moveImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sharebutton = new System.Windows.Forms.Button();
            this.SettingBtn = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SettingBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Location = new System.Drawing.Point(20, 94);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1320, 754);
            this.panel2.TabIndex = 17;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MidnightBlue;
            this.panel1.Controls.Add(this.sharebutton);
            this.panel1.Controls.Add(this.SettingBtn);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1358, 78);
            this.panel1.TabIndex = 16;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(92, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(320, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "PHOTO GALLERY";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.moveImagesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(201, 36);
            // 
            // moveImagesToolStripMenuItem
            // 
            this.moveImagesToolStripMenuItem.Image = global::PhotoGallery_Desktop.Properties.Resources.move_up__1_;
            this.moveImagesToolStripMenuItem.Name = "moveImagesToolStripMenuItem";
            this.moveImagesToolStripMenuItem.Size = new System.Drawing.Size(200, 32);
            this.moveImagesToolStripMenuItem.Text = "Move Images";
            this.moveImagesToolStripMenuItem.Click += new System.EventHandler(this.moveImagesToolStripMenuItem_Click);
            // 
            // sharebutton
            // 
            this.sharebutton.BackColor = System.Drawing.Color.AliceBlue;
            this.sharebutton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sharebutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sharebutton.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.sharebutton.Image = global::PhotoGallery_Desktop.Properties.Resources.move_up__1_;
            this.sharebutton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sharebutton.Location = new System.Drawing.Point(1100, 17);
            this.sharebutton.Name = "sharebutton";
            this.sharebutton.Size = new System.Drawing.Size(122, 51);
            this.sharebutton.TabIndex = 33;
            this.sharebutton.Text = "      Move";
            this.sharebutton.UseVisualStyleBackColor = false;
            this.sharebutton.Click += new System.EventHandler(this.sharebutton_Click);
            // 
            // SettingBtn
            // 
            this.SettingBtn.Image = global::PhotoGallery_Desktop.Properties.Resources.menu1;
            this.SettingBtn.Location = new System.Drawing.Point(1245, 9);
            this.SettingBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SettingBtn.Name = "SettingBtn";
            this.SettingBtn.Size = new System.Drawing.Size(57, 58);
            this.SettingBtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.SettingBtn.TabIndex = 1;
            this.SettingBtn.TabStop = false;
            this.SettingBtn.Click += new System.EventHandler(this.SettingBtn_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(20, 23);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(56, 37);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // SecondScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1353, 865);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SecondScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SecondScreen";
            this.Load += new System.EventHandler(this.SecondScreen_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SettingBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem moveImagesToolStripMenuItem;
        private System.Windows.Forms.PictureBox SettingBtn;
        private System.Windows.Forms.Button sharebutton;
    }
}