namespace PhotoGallery_Desktop
{
    partial class Taskcs
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
            this.Search = new System.Windows.Forms.Label();
            this.showimagegallery = new System.Windows.Forms.FlowLayoutPanel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.rbPicture = new System.Windows.Forms.RadioButton();
            this.rbFolder = new System.Windows.Forms.RadioButton();
            this.pbSearch = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // Search
            // 
            this.Search.AutoSize = true;
            this.Search.Location = new System.Drawing.Point(70, 39);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(60, 20);
            this.Search.TabIndex = 16;
            this.Search.Text = "Search";
            // 
            // showimagegallery
            // 
            this.showimagegallery.AutoScroll = true;
            this.showimagegallery.Location = new System.Drawing.Point(13, 156);
            this.showimagegallery.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.showimagegallery.Name = "showimagegallery";
            this.showimagegallery.Size = new System.Drawing.Size(774, 256);
            this.showimagegallery.TabIndex = 15;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(147, 39);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(276, 26);
            this.txtSearch.TabIndex = 14;
            // 
            // rbPicture
            // 
            this.rbPicture.AutoSize = true;
            this.rbPicture.Location = new System.Drawing.Point(74, 105);
            this.rbPicture.Name = "rbPicture";
            this.rbPicture.Size = new System.Drawing.Size(83, 24);
            this.rbPicture.TabIndex = 13;
            this.rbPicture.TabStop = true;
            this.rbPicture.Text = "Picture";
            this.rbPicture.UseVisualStyleBackColor = true;
            // 
            // rbFolder
            // 
            this.rbFolder.AutoSize = true;
            this.rbFolder.Location = new System.Drawing.Point(262, 105);
            this.rbFolder.Name = "rbFolder";
            this.rbFolder.Size = new System.Drawing.Size(79, 24);
            this.rbFolder.TabIndex = 12;
            this.rbFolder.TabStop = true;
            this.rbFolder.Text = "Folder";
            this.rbFolder.UseVisualStyleBackColor = true;
            // 
            // pbSearch
            // 
            this.pbSearch.Image = global::PhotoGallery_Desktop.Properties.Resources.search;
            this.pbSearch.Location = new System.Drawing.Point(430, 39);
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Size = new System.Drawing.Size(37, 26);
            this.pbSearch.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbSearch.TabIndex = 17;
            this.pbSearch.TabStop = false;
            this.pbSearch.Click += new System.EventHandler(this.pbSearch_Click_1);
            // 
            // Taskcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pbSearch);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.showimagegallery);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.rbPicture);
            this.Controls.Add(this.rbFolder);
            this.Name = "Taskcs";
            this.Text = "Taskcs";
            this.Load += new System.EventHandler(this.Taskcs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbSearch;
        private System.Windows.Forms.Label Search;
        private System.Windows.Forms.FlowLayoutPanel showimagegallery;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.RadioButton rbPicture;
        private System.Windows.Forms.RadioButton rbFolder;
    }
}