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

namespace PhotoGallery_Desktop
{
	public partial class SearchImage : Form
	{

        private GMapOverlay markersOverlay = new GMapOverlay("markers");
        private GMarkerGoogle currentMarker;
        public SearchImage()
		{
			InitializeComponent();

            //InitializeWebView();
            InitializeMap();

        }
        private void InitializeMap()
        {
            // Configure GMap
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerOnly; // Avoids cache issues
            gMapControl1.SetPositionByKeywords("Islamabad, Pakistan"); // Default location
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 20;
            gMapControl1.Zoom = 10;
            gMapControl1.Overlays.Add(markersOverlay);

            // Enable marker drag
            gMapControl1.MouseClick += GMapControl1_MouseClick;
        }

        private void GMapControl1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                PointLatLng point = gMapControl1.FromLocalToLatLng(e.X, e.Y);

                // Clear previous marker
                markersOverlay.Markers.Clear();

                // Add new marker
                currentMarker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
                currentMarker.IsVisible = true;
                currentMarker.IsHitTestVisible = true;
                currentMarker.ToolTipText = $"Lat: {point.Lat}, Lng: {point.Lng}";

                markersOverlay.Markers.Add(currentMarker);
            }
        }
        private void button1_Click(object sender, EventArgs e)
		{
            if (!string.IsNullOrWhiteSpace(txtPersonName.Text))
            {
                AddNameTag(txtPersonName.Text);
                txtPersonName.Clear();
            }

        }

        private void AddNameTag(string name)
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
            Button btnRemove = new Button();
            btnRemove.Text = "×"; // Better cross symbol
            btnRemove.ForeColor = Color.Black;
            btnRemove.BackColor = Color.Transparent;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.FlatAppearance.BorderSize = 0;
            btnRemove.Size = new Size(18, 18); // Smaller size
            btnRemove.Margin = new Padding(0, 1, 0, 1);
            btnRemove.Cursor = Cursors.Hand;
            btnRemove.Click += (s, e) => flowLayoutPanel1.Controls.Remove(namePanel);

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
            flowLayoutPanel1.Controls.Add(namePanel);
        }

        private void btnSearch_Click(object sender, EventArgs e)
		{

		}
		//private async void InitializeWebView()
		//{
		//	await webViewMap.EnsureCoreWebView2Async(null);
		//	webViewMap.CoreWebView2.Settings.IsWebMessageEnabled = true;
		//	LoadOpenStreetMap();
		//}

		//private void LoadOpenStreetMap()
		//{
		//	string html = @"
  //      <html>
  //      <head>
  //          <meta charset='utf-8' />
  //          <meta name='viewport' content='width=device-width, initial-scale=1.0' />
  //          <title>OpenStreetMap with Leaflet</title>
  //          <link rel='stylesheet' href='https://unpkg.com/leaflet/dist/leaflet.css' />
  //      </head>
  //      <body>
  //          <div id='map' style='width: 100%; height: 80vh;'></div>
  //          <script src='https://unpkg.com/leaflet/dist/leaflet.js'></script>
  //          <script>
  //              window.onload = function() {
  //                  var map = L.map('map').setView([30.865104, 430.180664], 5);
  //                  L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
  //                      attribution: '© OpenStreetMap contributors'
  //                  }).addTo(map);

  //                  var marker;
  //                  map.on('click', function(e) {
  //                      var lat = e.latlng.lat.toFixed(6);
  //                      var lng = e.latlng.lng.toFixed(6);

  //                      if (marker) {
  //                          map.removeLayer(marker);
  //                      }

  //                      marker = L.marker([lat, lng]).addTo(map);
  //                      if (window.chrome.webview) {
  //                          window.chrome.webview.postMessage(lat + ', ' + lng);
  //                      }
  //                  });
  //              };
  //          </script>
  //      </body>
  //      </html>";

		//	webViewMap.NavigateToString(html);
		//	webViewMap.WebMessageReceived += WebViewMap_WebMessageReceived;
		//}

		//private void WebViewMap_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
		//{
		//	string latLon = e.WebMessageAsJson.Trim('"');
		//	lblLocationName.Text = "Selected Coordinates: " + latLon;
		//}

        private void SearchImage_Load(object sender, EventArgs e)
        {

        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }
    }

}
