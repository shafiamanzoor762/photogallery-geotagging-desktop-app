namespace PhotoGallery_Desktop
{
	partial class AddNewEvent
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
			this.txtEventName = new System.Windows.Forms.TextBox();
			this.btnAddEvent = new System.Windows.Forms.Button();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtEventName
			// 
			this.txtEventName.Location = new System.Drawing.Point(95, 139);
			this.txtEventName.Name = "txtEventName";
			this.txtEventName.Size = new System.Drawing.Size(100, 20);
			this.txtEventName.TabIndex = 0;
			// 
			// btnAddEvent
			// 
			this.btnAddEvent.BackColor = System.Drawing.Color.Teal;
			this.btnAddEvent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAddEvent.ForeColor = System.Drawing.Color.White;
			this.btnAddEvent.Location = new System.Drawing.Point(110, 173);
			this.btnAddEvent.Name = "btnAddEvent";
			this.btnAddEvent.Size = new System.Drawing.Size(75, 23);
			this.btnAddEvent.TabIndex = 1;
			this.btnAddEvent.Text = "Add Event";
			this.btnAddEvent.UseVisualStyleBackColor = false;
			this.btnAddEvent.Click += new System.EventHandler(this.btnAddEvent_Click);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoScroll = true;
			this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 6);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(288, 99);
			this.flowLayoutPanel1.TabIndex = 29;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(100, 120);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(86, 13);
			this.label1.TabIndex = 30;
			this.label1.Text = "Enter new Event";
			// 
			// AddNewEvent
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(301, 213);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.btnAddEvent);
			this.Controls.Add(this.txtEventName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "AddNewEvent";
			this.Text = "AddNewEvent";
			this.Load += new System.EventHandler(this.AddNewEvent_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtEventName;
		private System.Windows.Forms.Button btnAddEvent;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Label label1;
	}
}