using BusinessLogicLayer;
using Newtonsoft.Json;
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
    public partial class Details : Form
    {
		private EditImageModel imageDetails;
		BindingList<Person> people = new BindingList<Person>();
		private int image_id;
		private Form previousForm;
		public Details(Form previousForm, int imageID)
        {
			this.previousForm = previousForm;
			this.image_id = imageID;
			InitializeComponent();
        }

		private async void Details_Load(object sender, EventArgs e)
		{
            label1.Visible = false;
            btnAddName.Visible = false;
            imageDetails = await Logic.GetImageDetails(image_id);
			
			if (imageDetails != null)
			{
				picBxImage.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.path));
				if (imageDetails.persons.Count() > 0)
				{
					picBxPerson1.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.persons[0].path));
                    label1.Visible = true;
                    btnAddName.Visible = true;
                }
				if (imageDetails.persons.Length > 1)
				{
					picBxPerson2.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.persons[1].path));
				}
                if (imageDetails.persons != null && imageDetails.persons.Length> 0)
                {
                    people = new BindingList<Person>(imageDetails.persons);
                }
                else
                {
                    MessageBox.Show("No persons found in the image data.");
                }

                List<Event> events = await Logic.FetchEventsAsync();
				
				foreach (var ev in imageDetails.events)
				{
					if (!string.IsNullOrWhiteSpace(ev.Name))
					{
						AddNameTag(ev.Name);
					}
				}
				if (imageDetails.event_date!=null)
					dtEventDateLbl.Text = imageDetails.event_date.ToString();
				if(imageDetails.capture_date!=null)
					dtCaptureDateLbl.Text = imageDetails.capture_date.ToString();
				if (imageDetails.location != null)
				{
					if(imageDetails.location.Name!=null)
					lblLocation.Text = imageDetails.location.Name;

				}
			}
			else
			{
				MessageBox.Show("Failed to load image details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void AddNameTag(string name)
		{

			// Create a rounded panel for the name tag
			Panel namePanel = new Panel();
			namePanel.BackColor = Color.MidnightBlue;
			namePanel.Padding = new Padding(6, 3, 6, 3);
			namePanel.Margin = new Padding(3);
			namePanel.Height = 22;
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


			// Add components
			innerPanel.Controls.Add(lblName);
			//innerPanel.Controls.Add(btnRemove);
			namePanel.Controls.Add(innerPanel);

			// Apply rounded corners
			namePanel.Paint += (s, e) =>
			{
				int radius = 10;
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

			// Add to FlowLayoutPanel
			flowLayoutPanel1.Controls.Add(namePanel);
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			this.Close(); // Close current form
			previousForm.Show();
		}

		private void btnAddName_Click(object sender, EventArgs e)
		{
			ShowPersonMetadata editPersonMetadata = new ShowPersonMetadata(people);
			editPersonMetadata.ShowDialog();
		}
	}
}
