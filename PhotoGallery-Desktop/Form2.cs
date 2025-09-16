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

namespace PhotoGallery_Desktop
{
    public partial class Form2 : Form
    {
        private GMapOverlay markersOverlay = new GMapOverlay("markers");
        private GMarkerGoogle currentMarker;

        public Form2()
        {
            InitializeComponent();
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



       

        private void btnsave_Click(object sender, EventArgs e)
        {
            if (currentMarker != null)
            {
                string location = $"Lat: {currentMarker.Position.Lat}, Lng: {currentMarker.Position.Lng}";
                MessageBox.Show($"Location saved: {location}");

                // You can save the coordinates to your photo metadata here
            }
            else
            {
                MessageBox.Show("No location selected!");
            }

        }

        private void btnsearch_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                var status = gMapControl1.SetPositionByKeywords(txtSearch.Text);
                if (status==null)
                {
                    MessageBox.Show("Location not found!");
                }
            }
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {

        }
    }
}
