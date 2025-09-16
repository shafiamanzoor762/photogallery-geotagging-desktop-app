using BusinessLogicLayer;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GMap.NET.MapProviders;
using GMap.NET;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace PhotoGallery_Desktop
{
    public partial class SearchImage : Form
    {

        private GMapOverlay markersOverlay = new GMapOverlay("markers");
        private GMarkerGoogle currentMarker;
        PointLatLng point;
        private Form previousForm;
        List<string> personNames = new List<string>();
        List<string> dates = new List<string>();
        List<string> events = new List<string>();
        List<string> selectedEvents = new List<string>();

        // Creating a list for gender
        List<string> genders = new List<string>();

        public SearchImage(Form previousForm)
        {
            InitializeComponent();
            this.previousForm = previousForm;

            //InitializeWebView();
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

        private void GMapControl1_MouseClick(object sender, MouseEventArgs e)
        {

            //if (e.Button == MouseButtons.Left)
            //{
            //    point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

            //    // Clear previous marker
            //    markersOverlay.Markers.Clear();

            //    // Add new marker
            //    currentMarker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
            //    currentMarker.IsVisible = true;
            //    currentMarker.IsHitTestVisible = true;
            //    currentMarker.ToolTipText = $"Lat: {point.Lat}, Lng: {point.Lng}";

            //    markersOverlay.Markers.Add(currentMarker);
            //}
        }
        // Person Names list
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPersonName.Text))
            {
                personNames.Add(txtPersonName.Text);

                AddNameTag(txtPersonName.Text);
                txtPersonName.Clear();
            }

        }

        private void AddNameTag(string name)
        {
            // Create a rounded panel for the name tag
            Panel namePanel = new Panel();
            namePanel.BackColor = Color.RoyalBlue;
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
            btnRemove.ForeColor = Color.Black;
            btnRemove.BackColor = Color.Transparent;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Size = new Size(18, 18);
            btnRemove.Margin = new Padding(0, 1, 0, 1);
            btnRemove.Cursor = Cursors.Hand;

            // ✅ Action when 'X' button is clicked
            btnRemove.Click += (s, e) =>
            {
                // Example action:
                flowLayoutPanel1.Controls.Remove(namePanel); // remove the tag visually

                // You can also remove it from a list or perform any other logic
                personNames.Remove(name); // Uncomment if you use a list
            };

            // Add label and button to the inner panel
            innerPanel.Controls.Add(lblName);
            innerPanel.Controls.Add(btnRemove);

            // Add inner panel to the outer name panel
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

            // Add to main container
            flowLayoutPanel1.Controls.Add(namePanel);
        }






        // Search images through BLL
        public async Task<List<string>> GetImagesthroughSearching()
        {
          
            genders.Clear(); // Clear before each search
            selectedEvents.Clear();

            if (chkbxMale.Checked) genders.Add("M");
            if (chkbxFemale.Checked) genders.Add("F");

            foreach (CheckBox cb in flowLayoutPanel2.Controls)
            {
                if (cb.Checked)
                    selectedEvents.Add(cb.Text);
            }



            ImageRequest requestData = new ImageRequest
            {
                name = new List<string>(personNames),
                gender = new List<string>(genders),
                selectedEvents = new List<string>(selectedEvents),
                capture_date = new List<string>(dates),
                age = int.TryParse(txtAge.Text, out int parsedAge) ? parsedAge : 0,

                location = new Location
                    {
                        Name = txtLocation.Text,
                        Latitude = (decimal)point.Lat,
                        Longitude = (decimal)point.Lng
                    }
                };

            // BLL method to search images
            return await Logic.SearchImagesAsync(requestData);
        }



        private async void btnSearch_Click(object sender, EventArgs e)
        {

            // Show the image panel on top
            LoadPanelimages.Visible = true;
            LoadPanelimages.BringToFront();

            // Disable AutoScroll during loading to prevent continuous scrolling
            LoadPanelimages.AutoScroll = false;

            List<string> imagePaths = await GetImagesthroughSearching();

            if (imagePaths != null && imagePaths.Count > 0)
            {
                LoadPanelimages.Controls.Clear(); // Clear old images

                int xOffset = 10;
                int yOffset = 50;  // Start from 50px to give space for the back button
                int rowHeight = 160;

                // Loop to display images
                foreach (var path in imagePaths)
                {
                    try
                    {
                        string fullUrl = "http://127.0.0.1:5000/images/" + path;
                        using (HttpClient client = new HttpClient())
                        {
                            byte[] imageBytes = await client.GetByteArrayAsync(fullUrl);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                PictureBox picBox = new PictureBox
                                {
                                    Image = Image.FromStream(ms),
                                    SizeMode = PictureBoxSizeMode.StretchImage,
                                    Width = 130,
                                    Height = 130,
                                    Location = new Point(xOffset, yOffset),
                                    Margin = new Padding(5),
                                    BorderStyle = BorderStyle.FixedSingle
                                };

                                LoadPanelimages.Controls.Add(picBox);

                                xOffset += 160;
                                if (xOffset + 150 > LoadPanelimages.Width)
                                {
                                    xOffset = 10;
                                    yOffset += rowHeight;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}");
                    }
                }

                // Create the Back Button AFTER all images
                Button backButton = new Button
                {
                    Text = "Back",
                    Size = new Size(80, 40),
                    BackColor = Color.PaleVioletRed,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Cursor = Cursors.Hand,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                };

                backButton.FlatAppearance.BorderSize = 0;
                backButton.FlatAppearance.MouseOverBackColor = Color.SteelBlue;
                backButton.Click += BackButton_Click;

                // Place the button at the top-left
                backButton.Location = new Point(10, 5);

                LoadPanelimages.Controls.Add(backButton);
            }
            else
            {
                MessageBox.Show("No images found for the selected search criteria.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Re-enable AutoScroll after loading is done
            LoadPanelimages.AutoScroll = true;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            // This code will run when the Back button is clicked
            LoadPanelimages.Visible = false; // Example: hide the panel
        }



        private async void SearchImage_Load(object sender, EventArgs e)
        {
            // events get from backend
            List<Event> allEvents = await Logic.FetchEventsAsync();
            if (allEvents != null)
            {
                foreach (var ev in allEvents)
                {
                    events.Add(ev.Name);
                }
            }
            AddCheckBoxes(events);
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }

        private void rdBtnFemale_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void dtPicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }


        private void AddCheckBoxes(List<string> items)
        {
            flowLayoutPanel2.Controls.Clear(); // Remove old checkboxes



            flowLayoutPanel2.FlowDirection = FlowDirection.TopDown; // Arrange checkboxes vertically
            flowLayoutPanel2.WrapContents = false; // Prevent wrapping in a row
            flowLayoutPanel2.AutoScroll = true; // Enable scrolling if items exceed panel size

            foreach (string item in items)
            {
                CheckBox chk = new CheckBox();
                chk.Text = item; // Set text from the list
                chk.AutoSize = true;
                chk.CheckedChanged += CheckBox_CheckedChanged; // Add event handler

                flowLayoutPanel2.Controls.Add(chk); // Add to FlowLayoutPanel
            }
        }
        // Event handler for checkbox click
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            if (chk != null)
            {
                if (chk.Checked)
                {
                    if (!selectedEvents.Contains(chk.Text))
                    {
                        selectedEvents.Add(chk.Text);
                    }
                }
                else
                {
                    if (selectedEvents.Contains(chk.Text))
                    {
                        selectedEvents.Remove(chk.Text);
                    }
                }
            }
        }


        private void AddNameTagDate(string name)
        {
            // Create a rounded panel for the name tag
            Panel namePanel = new Panel();
            namePanel.BackColor = Color.RoyalBlue;
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
            Button btnRemove = new Button();
            btnRemove.Text = "×"; // Better cross symbol
            btnRemove.ForeColor = Color.Black;
            btnRemove.BackColor = Color.Transparent;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Size = new Size(18, 18); // Smaller size
            btnRemove.Margin = new Padding(0, 1, 0, 1);
            btnRemove.Cursor = Cursors.Hand;

            // Button click event - handle remove and additional action
            btnRemove.Click += (s, e) =>
            {
               

                // Remove the name tag from the FlowPanel
                flowPanelDate.Controls.Remove(namePanel);
                dates.Remove(name); // Ensure you also remove it from the dates list
            };

            // Add components to the inner FlowLayoutPanel
            innerPanel.Controls.Add(lblName);
            innerPanel.Controls.Add(btnRemove);

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
            flowPanelDate.Controls.Add(namePanel);
        }


        private void button1_Click_1(object sender, EventArgs e)
        {

        }



        public class ImageResponse
        {
            public List<string> paths { get; set; }
        }

        private void LoadPanelimages_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chkbxMale_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LoadPanelimages_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void btnAddDate_Click_1(object sender, EventArgs e)
        {
            DateTime selectedDates = dtPicker.Value.Date;
            string selectedDate = selectedDates.ToString("yyyy-MM-dd"); // Format as "YYYY-MM-DD"


            if (!string.IsNullOrWhiteSpace(selectedDate))
            {
                AddNameTagDate(selectedDate);
                dates.Add(selectedDate);

            }
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); // Close current form
            previousForm.Show();
        }

        private void dtPicker_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void flowPanelDate_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanelDate_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Age_Click(object sender, EventArgs e)
        {

        }
    }

}
