using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using BusinessLogicLayer;
using System.Xml.Linq;
using System.Drawing.Drawing2D;

namespace PhotoGallery_Desktop
{
    public partial class EditImage : Form
    {
		// BRAND NEW
        private GMapOverlay markersOverlay = new GMapOverlay("markers");
        private GMarkerGoogle currentMarker;
        private EditImageModel imageDetails;
        BindingList<Person> people = new BindingList<Person>();
        public static BindingList<MergePeople> linkperson = new BindingList<MergePeople>();
        BindingList<string> eventList = new BindingList<string>();
		private double lat;
		private double lng;
		private int image_id;
        private Form previousForm;
        public EditImage(Form previousForm,int imageID)
        {
            this.previousForm = previousForm;
            this.image_id = imageID;
			InitializeComponent();
            InitializeMap();
		}
        private void InitializeMap()
        {
            //// Configure GMap
            //gMapControl1.MapProvider = GMapProviders.GoogleMap;
            //GMaps.Instance.Mode = AccessMode.ServerOnly; // Avoids cache issues
            ////gMapControl1.SetPositionByKeywords("Islamabad, Pakistan"); // Default location
            //gMapControl1.Position = new PointLatLng(lat, lng);
            //gMapControl1.MinZoom = 1;
            //gMapControl1.MaxZoom = 20;
            //gMapControl1.Zoom = 10;
            //gMapControl1.Overlays.Add(markersOverlay);

            //// Enable marker drag
            //gMapControl1.MouseClick += GMapControl1_MouseClick;
        }

        private async void GMapControl1_MouseClick(object sender, MouseEventArgs e)
        {

   //         if (e.Button == MouseButtons.Left)
   //         {
   //             PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

   //             // Clear previous marker
   //             markersOverlay.Markers.Clear();

   //             // Add new marker
   //             currentMarker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
   //             currentMarker.IsVisible = true;
   //             currentMarker.IsHitTestVisible = true;
   //             currentMarker.ToolTipText = $"Lat: {point.Lat}, Lng: {point.Lng}";
			//	lat = point.Lat;
   //             lng = point.Lng;
			//	markersOverlay.Markers.Add(currentMarker);
			////	lblLocation.Text = await Logic.GetLocationFromLatLonAsync(lat, lng);

			//}
        }

		private void btnAddName_Click(object sender, EventArgs e)
		{
			EditPersonMetadata editPersonMetadata = new EditPersonMetadata(people,linkperson);
			editPersonMetadata.ShowDialog();
		}

		private async void EditImage_Load(object sender, EventArgs e)
		{
			label1.Visible = false;
            btnAddName.Visible = false;
            if (image_id != null)
			{
				imageDetails = await Logic.GetImageDetails(image_id);

				if (imageDetails != null)
				{
					if (imageDetails.persons.Count() > 0)
					{
						picBxPerson1.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.persons[0].path));
						label1.Visible = true; // Show label if at least one person exists
                        btnAddName.Visible = true; // Enable button if at least one person exists
                    }
					if (imageDetails.persons.Length > 1)
					{
						picBxPerson2.Image = Image.FromStream(await Logic.LoadImageFromUrl(imageDetails.persons[1].path));
                    }
					people = new BindingList<Person>(imageDetails.persons);

					// Load events
					List<Event> events = await Logic.FetchEventsAsync();
					//comBxEvent.Items.AddRange(events.Select(ev => ev.Name).ToArray());
					foreach (var ev in events)
					{
						if (!string.IsNullOrWhiteSpace(ev.Name))
						{
							comBxEvent.Items.Add(ev.Name);

						}
					}
					foreach (var ev in imageDetails.events)
					{
						if (!string.IsNullOrWhiteSpace(ev.Name))
						{
							AddNameTag(ev.Name);
						}
					}
					if (imageDetails.event_date != null)
						dtEventDate.Value = (DateTime)imageDetails.event_date;
					//if (imageDetails.event_date != null)
					//	dtCaptureDate.Value = imageDetails.capture_date;
					if (imageDetails.location != null)
					{
						if (imageDetails.location.Name != null)
							//lblLocation.Text = imageDetails.location.Name;
						if (imageDetails.location.Latitude != null && imageDetails.location.Longitude != null)
						{
							lat = (double)imageDetails.location.Latitude;
							lng = (double)imageDetails.location.Longitude;
						}
					}
				}
				else
				{
					MessageBox.Show("Failed to load image details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private async void btnSave_ClickAsync(object sender, EventArgs e)
		{
			EditImageModel editImageModel = new EditImageModel();
			//Link linkmodel = new Link();
			editImageModel = imageDetails;
			editImageModel.persons = people.ToArray();
			//editImageModel.events = new Event[] { new Event { Name = comBxEvent.SelectedItem.ToString() } };
			List<Event> events = new List<Event>();
			foreach(var ev in eventList) 
			{
				events.Add(new Event { Id = 0, Name = ev});
			}
			editImageModel.events = events.ToArray();
			editImageModel.event_date = dtEventDate.Value;
			//editImageModel.capture_date = dtCaptureDate.Value;
			//editImageModel.location = new Location { Latitude = (decimal?)lat, Longitude = (decimal?)lng, Name = lblLocation.Text };
			//_ = await Logic.EditImageDataAsync(editImageModel);

			editImageModel.location = new Location { Latitude = (decimal?)lat, Longitude = (decimal?)lng, Name = txtlocation.Text };
			_ = await Logic.EditImageDataAsync(editImageModel);
			foreach (var mergepeople in linkperson)
			{
				int p1 = mergepeople.person1_id;
				string p2 = mergepeople.person2_emb_name;

				try
				{
					string result = await Logic.SaveMergepeopleDataAsync(p1, p2);
					Console.WriteLine($"Success: {result}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed for person {p1} - {ex.Message}");

				}
			}
                    MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void comBxEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedEvent = comBxEvent.SelectedItem.ToString();
			if (!string.IsNullOrWhiteSpace(selectedEvent))
			{
				AddNameTag(selectedEvent);
			}
		}

		private void AddNameTag(string name)
		{
			if (!eventList.Contains(name)) // Avoid duplicate names
			{
				eventList.Add(name);
			}

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

			// Create remove button (X)
			Button btnRemove = new Button();
			btnRemove.Text = "×";
			btnRemove.ForeColor = Color.White;
			btnRemove.BackColor = Color.Transparent;
			btnRemove.FlatStyle = FlatStyle.Flat;
			btnRemove.FlatAppearance.BorderSize = 0;
			btnRemove.Size = new Size(18, 18);
			btnRemove.Margin = new Padding(0, 1, 0, 1);
			btnRemove.Cursor = Cursors.Hand;

			// Remove both from UI and list
			btnRemove.Click += (s, e) =>
			{
				flowLayoutPanel1.Controls.Remove(namePanel);
				eventList.Remove(name); // Remove from BindingList
			};

			// Add components
			innerPanel.Controls.Add(lblName);
			innerPanel.Controls.Add(btnRemove);
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

		private async void button2_Click(object sender, EventArgs e)
		{
			AddNewEvent addNewEvent = new AddNewEvent();
			if (addNewEvent.ShowDialog() == DialogResult.OK)
			{
				// Load events
				List<Event> events = await Logic.FetchEventsAsync();
				comBxEvent.Items.Clear();
				//comBxEvent.Items.AddRange(events.Select(ev => ev.Name).ToArray());
				
				foreach (var ev in events)
				{
					if (!string.IsNullOrWhiteSpace(ev.Name))
					{
						comBxEvent.Items.Add(ev.Name);

					}
				}
			}
		}

		//////////////////////////////
		private void panel1_Paint(object sender, PaintEventArgs e)
		{

		}

		private void lblLocationName_Click(object sender, EventArgs e)
		{

		}

		private void label1_Click(object sender, EventArgs e)
		{

		}

		private void label5_Click(object sender, EventArgs e)
		{

		}

		private void label6_Click(object sender, EventArgs e)
		{

		}

		private void gMapControl1_Load(object sender, EventArgs e)
		{

		}

        private void pictureBox2_Click(object sender, EventArgs e)
        {
			this.Close();
            previousForm.Show();
        }

        private void lblLocation_Click(object sender, EventArgs e)
        {

        }
    }
}
