using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoGallery_Desktop
{
	public partial class AddNewEvent : Form
	{
		public AddNewEvent()
		{
			InitializeComponent();
		}

		private async void AddNewEvent_Load(object sender, EventArgs e)
		{
			List<Event> events = await Logic.FetchEventsAsync();
			foreach (var ev in events)
			{
				if (!string.IsNullOrWhiteSpace(ev.Name))
				{
					AddEventTag(ev.Name);
				}
			}
			
		}

		private async void btnAddEvent_Click(object sender, EventArgs e)
		{
			string resp = Logic.AddNewEventAsync(txtEventName.Text).ToString();
			//MessageBox.Show(resp, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			AddEventTag(txtEventName.Text);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void AddEventTag(string name)
		{
			// Create a rounded panel for the name tag
			Panel namePanel = new Panel();
			namePanel.BackColor = Color.Teal;
			namePanel.Padding = new Padding(6, 3, 6, 3); // Adjust padding
			namePanel.Margin = new Padding(3);
			namePanel.Height = 22; // Decreased height for a sleek look
			namePanel.AutoSize = true;
			namePanel.MinimumSize = new Size(50, 22);
			namePanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;

			// Use FlowLayoutPanel for automatic alignment
			FlowLayoutPanel innerPanel = new FlowLayoutPanel();
			innerPanel.AutoSize = true;
			innerPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			innerPanel.FlowDirection = FlowDirection.LeftToRight;
			innerPanel.WrapContents = false;
			innerPanel.BackColor = Color.Transparent;

			// Create label for name
			Label lblName = new Label();
			lblName.Text = name;
			lblName.ForeColor = Color.White;
			lblName.AutoSize = true;
			lblName.Margin = new Padding(0, 2, 5, 2);

			// Create remove button (X) with transparent background and black color
			//Button btnRemove = new Button();
			//btnRemove.Text = "×"; // Better cross symbol
			//btnRemove.ForeColor = Color.Black;
			//btnRemove.BackColor = Color.Transparent;
			//btnRemove.FlatStyle = FlatStyle.Flat;
			//btnRemove.FlatAppearance.BorderSize = 0;
			//btnRemove.Size = new Size(18, 18); // Smaller size
			//btnRemove.Margin = new Padding(0, 1, 0, 1);
			//btnRemove.Cursor = Cursors.Hand;
			//btnRemove.Click += (s, e) => flowLayoutPanel1.Controls.Remove(namePanel);

			// Add components to the inner FlowLayoutPanel
			innerPanel.Controls.Add(lblName);
			//innerPanel.Controls.Add(btnRemove);

			// Add the inner panel to the namePanel
			namePanel.Controls.Add(innerPanel);

			// Apply smoother rounded corners using OnPaint
			namePanel.Paint += (s, e) =>
			{
				int radius = 10; // Smaller radius for smooth edges
				using (GraphicsPath path = new GraphicsPath())
				{
					path.AddArc(0, 0, radius, radius, 180, 90);
					path.AddArc(namePanel.Width - radius - 1, 0, radius, radius, 270, 90);
					path.AddArc(namePanel.Width - radius - 1, namePanel.Height - radius - 1, radius, radius, 0, 90);
					path.AddArc(0, namePanel.Height - radius - 1, radius, radius, 90, 90);
					path.CloseFigure();
					namePanel.Region = new Region(path);
				}
			};

			// Add namePanel to the FlowLayoutPanel
			flowLayoutPanel1.Controls.Add(namePanel);
		}
	}
}
