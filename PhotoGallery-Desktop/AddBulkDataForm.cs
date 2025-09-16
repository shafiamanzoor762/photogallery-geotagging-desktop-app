using BusinessLogicLayer;
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
using System.Drawing.Drawing2D;

namespace PhotoGallery_Desktop
{
	public partial class AddBulkDataForm : Form
	{
		private GMapOverlay markersOverlay = new GMapOverlay("markers");
		private GMarkerGoogle currentMarker;
		//PointLatLng point;
		private double lat;
		private double lng;

		private string[] parts;
		public string secondPart;
		public string firstPart;

		BindingList<string> eventList = new BindingList<string>();
		List<EditImageModel> selectedImages = new List<EditImageModel>();
		public AddBulkDataForm(List<EditImageModel> selectedImages)
		{
			this.selectedImages = selectedImages;
			InitializeComponent();
			InitializeMap();
		}

		private void InitializeMap()
		{
			// Configure GMap
			//gMapControl1.MapProvider = GMapProviders.GoogleMap;
			//GMaps.Instance.Mode = AccessMode.ServerOnly; // Avoids cache issues
			//gMapControl1.SetPositionByKeywords("Islamabad, Pakistan"); // Default location
			//gMapControl1.MinZoom = 1;
			//gMapControl1.MaxZoom = 20;
			//gMapControl1.Zoom = 10;
			//gMapControl1.Overlays.Add(markersOverlay);

			//// Enable marker drag
			//gMapControl1.MouseClick += GMapControl1_MouseClick;
		}

		private async void GMapControl1_MouseClick(object sender, MouseEventArgs e)
		{

			//if (e.Button == MouseButtons.Left)
			//{
			//	PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

			//	// Clear previous marker
			//	markersOverlay.Markers.Clear();

			//	// Add new marker
			//	currentMarker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
			//	currentMarker.IsVisible = true;
			//	currentMarker.IsHitTestVisible = true;
			//	currentMarker.ToolTipText = $"Lat: {point.Lat}, Lng: {point.Lng}";
			//	lat = point.Lat;
			//	lng = point.Lng;
			//	markersOverlay.Markers.Add(currentMarker);
			//	lblLocation.Text = await Logic.GetLocationFromLatLonAsync(lat, lng);

			//}
		}


		private async void btnSave_Click(object sender, EventArgs e)
		{
			try
			{
				foreach (var editImageModel in selectedImages)
				{
					List<Event> events = new List<Event>();
					foreach (var ev in eventList)
					{
						events.Add(new Event { Id = 0, Name = ev });
					}
					editImageModel.events = events.ToArray();
					editImageModel.event_date = dtEventDate.Value;
					//editImageModel.location = new Location { Latitude = (decimal?)lat, Longitude = (decimal?)lng, Name = lblLocation.Text };
					//_ = await Logic.EditImageDataAsync(editImageModel);

					editImageModel.location = new Location { Latitude = (decimal?)lat, Longitude = (decimal?)lng, Name = txtlocation.Text };
					_ = await Logic.EditImageDataAsync(editImageModel);
				}
                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

			}
		}

		private async void button2_Click(object sender, EventArgs e)
		{
			AddNewEvent addNewEvent = new AddNewEvent();
			if (addNewEvent.ShowDialog() == DialogResult.OK)
			{
				// Load events
				List<Event> events = await Logic.FetchEventsAsync();
				comBxEvent.Items.Clear();
				foreach (var ev in events)
				{
					if (!string.IsNullOrWhiteSpace(ev.Name))
					{
						comBxEvent.Items.Add(ev.Name);

					}
				}
			}
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

		private async void AddBulkDataForm_Load(object sender, EventArgs e)
		{
			List<Event> events = await Logic.FetchEventsAsync();
			//comBxEvent.Items.AddRange(events.Select(ev => ev.Name).ToArray());
			foreach (var ev in events)
			{
				if (!string.IsNullOrWhiteSpace(ev.Name))
				{
					comBxEvent.Items.Add(ev.Name);

				}
			}
		}

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
