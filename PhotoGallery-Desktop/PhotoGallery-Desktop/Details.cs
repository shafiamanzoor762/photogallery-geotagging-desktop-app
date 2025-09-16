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

			imageDetails = await Logic.GetImageDetails(image_id);
			
			if (imageDetails != null)
			{
				picBxImage.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.path));
				if (imageDetails.persons.Count() > 0)
				{
					picBxPerson1.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.persons[0].path));
				}
				if (imageDetails.persons.Length > 1)
				{
					picBxPerson2.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.persons[1].path));
				}
				people = new BindingList<Person>(imageDetails.persons);

				foreach(var person in imageDetails.persons)
				{
					AddRoundedImageToFlowPanel(person);
				}
				// Load events
				List<Event> events = await Logic.FetchEventsAsync();
				//comBxEvent.Items.AddRange(events.Select(ev => ev.Name).ToArray());
				//foreach (var ev in events)
				//{
				//	if (!string.IsNullOrWhiteSpace(ev.Name))
				//	{
				//		comBxEvent.Items.Add(ev.Name);

				//	}
				//}
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

					//lat = (double)imageDetails.location.Latitude;
					//lng = (double)imageDetails.location.Longitude;
				}
			}
			else
			{
				MessageBox.Show("Failed to load image details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void AddNameTag(string name)
		{
			//if (!eventList.Contains(name)) // Avoid duplicate names
			//{
			//	eventList.Add(name);
			//}

			// Create a rounded panel for the name tag
			Panel namePanel = new Panel();
			namePanel.BackColor = Color.Teal;
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

			// Create remove button (X)
			//Button btnRemove = new Button();
			//btnRemove.Text = "×";
			//btnRemove.ForeColor = Color.Black;
			//btnRemove.BackColor = Color.Transparent;
			//btnRemove.FlatStyle = FlatStyle.Flat;
			//btnRemove.FlatAppearance.BorderSize = 0;
			//btnRemove.Size = new Size(18, 18);
			//btnRemove.Margin = new Padding(0, 1, 0, 1);
			//btnRemove.Cursor = Cursors.Hand;

			//// Remove both from UI and list
			//btnRemove.Click += (s, e) =>
			//{
			//	flowLayoutPanel1.Controls.Remove(namePanel);
			//	eventList.Remove(name); // Remove from BindingList
			//};

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

		private async void AddRoundedImageToFlowPanel(Person person)
		{
			PictureBox picBox = new PictureBox
			{
				Size = new Size(25, 25), // Fixed size
				SizeMode = PictureBoxSizeMode.StretchImage, // Adjust image
				Image = Image.FromStream(await Logic.LoadImageFromUrl(person.path))
			};
			picBox.Tag = "person_id:" + person.id + ";" + person.name;
			//JsonConvert.SerializeObject(person);
			picBox.Paint += PictureBox_Paint;
			picBox.Click += picBxPerson1_Click; // Event handler for click
			flowPnlPerson.Controls.Add(picBox);

		}

		//private async void picBxPerson1_Click(object sender, EventArgs e)
		//{
		//	PictureBox pictureBox = sender as PictureBox;
		//	if (pictureBox != null && pictureBox.Tag != null)
		//	{
		//		string json = pictureBox.Tag.ToString();
		//		Person person = JsonConvert.DeserializeObject<Person>(json);
		//		List<ImageDetailsModel> images = await Logic.FetchPersonImages(person.id);
		//	}
		//}

		private void picBxPerson1_Click(object sender, EventArgs e)
		{
			if (sender is PictureBox pictureBox && pictureBox.Tag is string eventName)
			{
				this.Hide();
				SecondScreen form = new SecondScreen(this, eventName);
				form.Show();
			}
		}

		private void PictureBox_Paint(object sender, PaintEventArgs e)
		{
			PictureBox pictureBox = sender as PictureBox;
			if (pictureBox == null) return;

			// Create a GraphicsPath to define rounded corners
			using (GraphicsPath path = new GraphicsPath())
			{
				int radius = 20; // Adjust corner radius
				path.AddArc(0, 0, radius, radius, 180, 90);
				path.AddArc(pictureBox.Width - radius, 0, radius, radius, 270, 90);
				path.AddArc(pictureBox.Width - radius, pictureBox.Height - radius, radius, radius, 0, 90);
				path.AddArc(0, pictureBox.Height - radius, radius, radius, 90, 90);
				path.CloseFigure();
				pictureBox.Region = new Region(path); // Apply rounded region

				// Draw the black rounded border
				using (Pen borderPen = new Pen(Color.Black, 2)) // Black border with width 4
				{
					e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Smooths edges
					e.Graphics.DrawPath(borderPen, path); // Draw the border along the path
				}
			}
		}

		//private Image GetRoundedImage(Image original, int diameter)
		//{
		//	Bitmap roundedBitmap = new Bitmap(diameter, diameter);
		//	using (Graphics g = Graphics.FromImage(roundedBitmap))
		//	{
		//		g.SmoothingMode = SmoothingMode.AntiAlias;
		//		using (GraphicsPath path = new GraphicsPath())
		//		{
		//			path.AddEllipse(0, 0, diameter, diameter);
		//			g.SetClip(path);
		//			g.DrawImage(original, 0, 0, diameter, diameter);
		//		}
		//	}
		//	return roundedBitmap;
		//}

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
