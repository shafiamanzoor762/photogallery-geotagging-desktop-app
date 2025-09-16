using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer;
using System.Windows.Forms;
using System.IO;
using System.Net.Http;
using System.IO.Ports;
using System.Drawing.Drawing2D;
using Newtonsoft.Json;

namespace PhotoGallery_Desktop
{
    public partial class EDDB : Form
    {
        private int id;
        private Form previousForm;

        public EDDB()
        {
            InitializeComponent();
        }
       
           


           

       public EDDB(Form previousForm, int imageid)
        {
            InitializeComponent();
            id = imageid;
            this.id= imageid;

            this.previousForm = previousForm;
        }

        private async void EDDB_Load(object sender, EventArgs e)
        {
            Logic l=new Logic();
            ImageDetailsModel imageDetails = await l.FetchImageDetails(id); // Fetch details from API

            if (imageDetails != null)
            {
                string fullUrl = imageDetails.path;

                using (HttpClient client = new HttpClient())
                {
                    byte[] imageBytes = File.ReadAllBytes(fullUrl);
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        picbximage.Image = Image.FromStream(ms); // Assuming `pictureBox1` exists
                        picbximage.SizeMode = PictureBoxSizeMode.Zoom; // Adjust the image display
                    }
                }


				//MessageBox.Show($"ID: {imageDetails.id}\nPath: {imageDetails.path}\n" +
				//                $"Captured: {imageDetails.capture_date}\n" +
				//                $"Event Date: {imageDetails.event_date}\n" +
				//                $"Last Modified: {imageDetails.last_modified}\n" +
				//                $"Location ID: {imageDetails.location_id}\n" +
				//                $"Synced: {imageDetails.is_sync}",
				//                "Image Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
				
				showCircleFaces();
			}
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.Hide();
            EditImage editImageForm = new EditImage(this,id);
            editImageForm.Show();
		}

        private void btnDetails_Click(object sender, EventArgs e)
        {
            Details detailsForm = new Details(this,id);
            detailsForm.Show(); // Show the Details Form as a new window
            this.Hide();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = this.id; // Extract ID
                              // EDDB eddb = new EDDB(id);
                              //  string imagepath = this.path;

            Delete deleteForm = new Delete(this, id); //path);
            deleteForm.Show(); // Show the Details Form as a new window
            this.Hide();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Show();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); // Close current form
            previousForm.Show();
        }


		//--------To Show circle crop faces for people in image -------------------
		// To redirect Traning images
		public async void showCircleFaces()
		{
			EditImageModel imageDetails = await Logic.GetImageDetails(id);
			if (imageDetails.persons.Count() > 0)
			{
				foreach (var person in imageDetails.persons)
				{
					AddRoundedImageToFlowPanel(person);
				}
			}
		}
		private void picBxPerson1_Click(object sender, EventArgs e)
		{
			if (sender is PictureBox pictureBox && pictureBox.Tag is string tagString)
			{
				this.Hide();
				Person people = JsonConvert.DeserializeObject<Person>(tagString);
				ShowTraningImages form = new ShowTraningImages(this, people);
				form.Show();
			}
		}



        //private async void AddRoundedImageToFlowPanel(Person person)
        //{
        //	PictureBox picBox = new PictureBox
        //	{
        //		Size = new Size(25, 25), // Fixed size
        //		SizeMode = PictureBoxSizeMode.StretchImage, // Adjust image
        //		Image = Image.FromStream(await Logic.LoadImageFromUrl(person.path))
        //	};
        //	picBox.Tag = JsonConvert.SerializeObject(person);
        //	//JsonConvert.SerializeObject(person);
        //	picBox.Paint += PictureBox_Paint;
        //	picBox.Click += picBxPerson1_Click; // Event handler for click
        //	flowPnlPerson.Controls.Add(picBox);

        //}

        private async void AddRoundedImageToFlowPanel(Person person)
        {
            PictureBox picBox = new PictureBox
            {
                Size = new Size(30, 30), // Slightly larger to better show overlay
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromStream(await Logic.LoadImageFromUrl(person.path))
            };

            picBox.Tag = JsonConvert.SerializeObject(person);
            picBox.Paint += PictureBox_Paint;

            if (person.name == "unknown")
            {
                // Attach paint event to draw overlay
                picBox.Paint += (s, e) =>
                {
                    // Load the overlay image from Resources
                    Image questionMark = Properties.Resources.question_mark; // Assuming you added it as "question_mark.png"

                    // Set overlay size (e.g., 20x20) and position (bottom-right corner)
                    int overlaySize = 12;
                    int x = picBox.Width - overlaySize - 0;
                    int y = picBox.Height - overlaySize - 0;

                    e.Graphics.DrawImage(questionMark, new Rectangle(x, y, overlaySize, overlaySize));
                };
            }

            picBox.Click += picBxPerson1_Click;
            flowPnlPerson.Controls.Add(picBox);
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

	}
}
