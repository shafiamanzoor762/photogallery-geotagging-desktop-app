using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoGallery_Desktop
{
    public partial class Taskcs : Form
    {
        public Taskcs()
        {
            InitializeComponent();
        }

        public async Task<List<string>> SearchImagesByName(string personName)
        {
            var nameList = new List<string> { personName };

            ImageRequest req = new ImageRequest
            {
                name = nameList,
                gender = new List<string>(),             // not filtering gender
                selectedEvents = new List<string>(),     // not filtering events
                capture_date = new List<string>(),       // no date filter
                age = 0,                                 // no age filter
                location = new Location()                // location ignored
            };

            return await Logic.SearchImagesAsync(req);   // same Logic class as SearchImage form
        }










        private void showimagegallery_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Taskcs_Load(object sender, EventArgs e)
        {

        }

        private void rbFolder_CheckedChanged(object sender, EventArgs e)
        {

        }

        private async void pbSearch_Click_1(object sender, EventArgs e)
        {


            string name = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter a name to search.", "Missing Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rbPicture.Checked)
            {
                //MessageBox.Show("Please select 'Picture' option to proceed.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //return;


                // Call backend to get images by name
                List<string> images = await SearchImagesByName(name);

                if (images != null && images.Count > 0)
                {
                    showimagegallery.Controls.Clear();

                    int x = 10;
                    int y = 10;
                    int rowHeight = 140;

                    foreach (string path in images)
                    {
                        try
                        {
                            string fullUrl = "http://127.0.0.1:5000/images/" + path;

                            using (HttpClient client = new HttpClient())
                            {
                                byte[] imgBytes = await client.GetByteArrayAsync(fullUrl);
                                using (MemoryStream ms = new MemoryStream(imgBytes))
                                {
                                    PictureBox pic = new PictureBox
                                    {
                                        Image = Image.FromStream(ms),
                                        SizeMode = PictureBoxSizeMode.StretchImage,
                                        Width = 130,
                                        Height = 130,
                                        Location = new Point(x, y),
                                        BorderStyle = BorderStyle.FixedSingle
                                    };

                                    showimagegallery.Controls.Add(pic);

                                    x += 140;
                                    if (x + 130 > showimagegallery.Width)
                                    {
                                        x = 10;
                                        y += rowHeight;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to load image: " + ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No images found for this person.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            // ✅ Yeh block sirf tab chale jab Folder radio button selected ho
            if (rbFolder.Checked)
            {
                var groupedImages = await Logic.GetImagesGroupedByEventAsync(name);
                showimagegallery.Controls.Clear();

                // Check if all groupings are empty
                bool hasNoImages =
                    (groupedImages.events == null || groupedImages.events.Count == 0) &&
                    (groupedImages.locations == null || groupedImages.locations.Count == 0) &&
                    (groupedImages.capture_dates == null || groupedImages.capture_dates.Count == 0) &&
                    (groupedImages.event_dates == null || groupedImages.event_dates.Count == 0);

                if (hasNoImages)
                {
                    MessageBox.Show("No images found for this person.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int y = 10;

                void AddFolderButtons(Dictionary<string, List<string>> groupDict)
                {
                    if (groupDict == null) return;

                    foreach (var group in groupDict)
                    {
                        Button btnEvent = new Button
                        {
                            Text = "📁 " + group.Key,
                            Location = new Point(10, y),
                            Size = new Size(250, 40),
                            Font = new Font("Arial", 10),
                            Tag = group.Value,
                            BackColor = Color.LightYellow
                        };

                        btnEvent.Click += async (s, evt) =>
                        {
                            List<string> images = btnEvent.Tag as List<string>;
                            showimagegallery.Controls.Clear();

                            // Back button
                            Button btnBack = new Button
                            {
                                Text = "🔙 Back",
                                Location = new Point(10, 10),
                                Size = new Size(100, 30),
                                BackColor = Color.LightGray
                            };
                            btnBack.Click += (s2, e2) => pbSearch_Click_1(null, null);
                            showimagegallery.Controls.Add(btnBack);

                            int x = 10, yImg = 50;
                            int rowHeight = 140;

                            foreach (var imagePath in images)
                            {
                                try
                                {
                                    string fullUrl = "http://127.0.0.1:5000/images/" + imagePath;
                                    using (HttpClient client = new HttpClient())
                                    {
                                        byte[] imgBytes = await client.GetByteArrayAsync(fullUrl);
                                        using (MemoryStream ms = new MemoryStream(imgBytes))
                                        {
                                            PictureBox pic = new PictureBox
                                            {
                                                Image = Image.FromStream(ms),
                                                SizeMode = PictureBoxSizeMode.StretchImage,
                                                Width = 130,
                                                Height = 130,
                                                Location = new Point(x, yImg),
                                                BorderStyle = BorderStyle.FixedSingle
                                            };
                                            showimagegallery.Controls.Add(pic);

                                            x += 140;
                                            if (x + 130 > showimagegallery.Width)
                                            {
                                                x = 10;
                                                yImg += rowHeight;
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Failed to load image: " + ex.Message);
                                }
                            }
                        };

                        showimagegallery.Controls.Add(btnEvent);
                        y += 60;
                    }
                }

                // Add folders from all available categories
                AddFolderButtons(groupedImages.events);
                AddFolderButtons(groupedImages.locations);
                AddFolderButtons(groupedImages.capture_dates);
                AddFolderButtons(groupedImages.event_dates);
            }

        }








       
    }
}
