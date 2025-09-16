namespace PhotoGallery_Desktop
{
    partial class Details
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Details));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLocation = new System.Windows.Forms.Label();
            this.btnAddName = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.picBxPerson2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.picBxPerson1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.picBxImage = new System.Windows.Forms.PictureBox();
            this.flowPnlPerson = new System.Windows.Forms.FlowLayoutPanel();
            this.dtEventDateLbl = new System.Windows.Forms.Label();
            this.dtCaptureDateLbl = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxPerson2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxPerson1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(59, 332);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(359, 64);
            this.flowLayoutPanel1.TabIndex = 71;
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocation.Location = new System.Drawing.Point(57, 650);
            this.lblLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(147, 25);
            this.lblLocation.TabIndex = 69;
            this.lblLocation.Text = "Islamabad Park";
            // 
            // btnAddName
            // 
            this.btnAddName.BackColor = System.Drawing.Color.White;
            this.btnAddName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddName.ForeColor = System.Drawing.Color.Black;
            this.btnAddName.Location = new System.Drawing.Point(72, 229);
            this.btnAddName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAddName.Name = "btnAddName";
            this.btnAddName.Size = new System.Drawing.Size(61, 27);
            this.btnAddName.TabIndex = 54;
            this.btnAddName.Text = "More";
            this.btnAddName.UseVisualStyleBackColor = false;
            this.btnAddName.Click += new System.EventHandler(this.btnAddName_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.RoyalBlue;
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(-8, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 63);
            this.panel1.TabIndex = 57;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1016, 12);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 41);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 30;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(24, 21);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(49, 30);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 29;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Image = global::PhotoGallery_Desktop.Properties.Resources.detail;
            this.label6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.Location = new System.Drawing.Point(96, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(253, 36);
            this.label6.TabIndex = 1;
            this.label6.Text = "DetailsImage";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picBxPerson2
            // 
            this.picBxPerson2.Location = new System.Drawing.Point(183, 139);
            this.picBxPerson2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picBxPerson2.Name = "picBxPerson2";
            this.picBxPerson2.Size = new System.Drawing.Size(80, 74);
            this.picBxPerson2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBxPerson2.TabIndex = 70;
            this.picBxPerson2.TabStop = false;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Image = global::PhotoGallery_Desktop.Properties.Resources.date;
            this.label5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label5.Location = new System.Drawing.Point(59, 418);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 30);
            this.label5.TabIndex = 65;
            this.label5.Text = "Event Date";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picBxPerson1
            // 
            this.picBxPerson1.Location = new System.Drawing.Point(71, 139);
            this.picBxPerson1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picBxPerson1.Name = "picBxPerson1";
            this.picBxPerson1.Size = new System.Drawing.Size(80, 74);
            this.picBxPerson1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBxPerson1.TabIndex = 63;
            this.picBxPerson1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Image = global::PhotoGallery_Desktop.Properties.Resources.date;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(59, 510);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(171, 30);
            this.label4.TabIndex = 62;
            this.label4.Text = "Capture Date";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Image = global::PhotoGallery_Desktop.Properties.Resources._event;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.Location = new System.Drawing.Point(59, 283);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 30);
            this.label2.TabIndex = 61;
            this.label2.Text = "Event";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Image = global::PhotoGallery_Desktop.Properties.Resources.person;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Location = new System.Drawing.Point(59, 91);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 30);
            this.label1.TabIndex = 60;
            this.label1.Text = "Person Name";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.Location = new System.Drawing.Point(59, 597);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(137, 39);
            this.label3.TabIndex = 59;
            this.label3.Text = "Location";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // picBxImage
            // 
            this.picBxImage.Location = new System.Drawing.Point(547, 91);
            this.picBxImage.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picBxImage.Name = "picBxImage";
            this.picBxImage.Size = new System.Drawing.Size(492, 460);
            this.picBxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBxImage.TabIndex = 72;
            this.picBxImage.TabStop = false;
            // 
            // flowPnlPerson
            // 
            this.flowPnlPerson.Location = new System.Drawing.Point(547, 574);
            this.flowPnlPerson.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.flowPnlPerson.Name = "flowPnlPerson";
            this.flowPnlPerson.Size = new System.Drawing.Size(453, 55);
            this.flowPnlPerson.TabIndex = 73;
            // 
            // dtEventDateLbl
            // 
            this.dtEventDateLbl.AutoSize = true;
            this.dtEventDateLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtEventDateLbl.Location = new System.Drawing.Point(59, 462);
            this.dtEventDateLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dtEventDateLbl.Name = "dtEventDateLbl";
            this.dtEventDateLbl.Size = new System.Drawing.Size(64, 25);
            this.dtEventDateLbl.TabIndex = 74;
            this.dtEventDateLbl.Text = "label7";
            // 
            // dtCaptureDateLbl
            // 
            this.dtCaptureDateLbl.AutoSize = true;
            this.dtCaptureDateLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtCaptureDateLbl.Location = new System.Drawing.Point(60, 553);
            this.dtCaptureDateLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.dtCaptureDateLbl.Name = "dtCaptureDateLbl";
            this.dtCaptureDateLbl.Size = new System.Drawing.Size(64, 25);
            this.dtCaptureDateLbl.TabIndex = 75;
            this.dtCaptureDateLbl.Text = "label7";
            // 
            // Details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 699);
            this.Controls.Add(this.dtCaptureDateLbl);
            this.Controls.Add(this.dtEventDateLbl);
            this.Controls.Add(this.flowPnlPerson);
            this.Controls.Add(this.picBxImage);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.picBxPerson2);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.picBxPerson1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnAddName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Details";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Details";
            this.Load += new System.EventHandler(this.Details_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxPerson2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxPerson1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.PictureBox picBxPerson2;
		private System.Windows.Forms.Label lblLocation;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.PictureBox picBxPerson1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button btnAddName;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.PictureBox picBxImage;
		private System.Windows.Forms.FlowLayoutPanel flowPnlPerson;
		private System.Windows.Forms.Label dtEventDateLbl;
		private System.Windows.Forms.Label dtCaptureDateLbl;
	}
}