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
    public partial class EditImage : Form
    {

        private GMapOverlay markersOverlay = new GMapOverlay("markers");
        private GMarkerGoogle currentMarker;
        public EditImage()
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
    }
}
