using BusinessLogicLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoGallery_Desktop
{
    public partial class GetPersonsGroupFromImage : Form
    {
        public GetPersonsGroupFromImage()
        {
            InitializeComponent();
        }

        private async void btnUploadImage_Click(object sender, EventArgs e)
        {
            txtOutput.Text = string.Empty;
            panlFaces.Controls.Clear();
            panel2.Controls.Clear();

            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath1 = dialog.FileName;

                    pictureBox1.Image = Image.FromFile(imagePath1);
                    txtStatus.Text = "Uploading...";

                    string response = await Logic.SendImageAsync(imagePath1);

                    txtStatus.Text = "Response received.";
                    txtOutput.Text = response;

                    
                    var fullResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);
                    if (fullResponse != null)
                    {
                        var extractedFaces = JsonConvert.DeserializeObject<List<string>>(fullResponse["extracted_faces"].ToString());
                        var matchedGroups = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(fullResponse["matched_groups"].ToString());
                        foreach (var p in extractedFaces)
                        {
                            AddRoundedImageToFlowPanel(Logic.faces + p.Replace("stored-faces", "face_images"));
                        }
                        try
                        {
                            // Check if the DataTable is null
                            if (matchedGroups == null)
                            {
                                MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else if (matchedGroups is List<Dictionary<string, object>> listData)
                            {
                                // Create a single FlowLayoutPanel to hold all items
                                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                                {
                                    Dock = DockStyle.Fill, // Fill the panel
                                    AutoScroll = true,     // Enable scrolling
                                    FlowDirection = FlowDirection.LeftToRight, // Arrange items vertically
                                    WrapContents = true   // Prevent wrapping to a new column
                                };
                                panel2.Controls.Add(flowLayoutPanel);

                                // Iterate over the listData


                                foreach (var dictionary in listData)  // Iterate over each person entry
                                {
                                    if (dictionary.ContainsKey("Person") && dictionary.ContainsKey("Images"))
                                    {
                                        var personData = dictionary["Person"] as JObject;
                                        var imagesArray = dictionary["Images"] as JArray;

                                        if (imagesArray != null && personData != null)
                                        {
                                            string personName = personData["name"]?.ToString() ?? "Unknown";  // Extract person's name
                                            string personid = personData["id"]?.ToString();
                                            string path = personData["path"]?.ToString();
                                            foreach (var item in imagesArray)
                                            {
                                                try
                                                {
                                                    var imageDetails = item.ToObject<Dictionary<string, object>>();

                                                    if (imageDetails != null)
                                                    {
                                                        // Extract image path from the image details
                                                        string imagePath = imageDetails["path"].ToString();
                                                        string imageurl = Logic.imageurl;  // Assuming this contains the base URL for images
                                                        string faces = Logic.faces;
                                                        // Create PictureBox for image
                                                        PictureBox pictureBox = new PictureBox
                                                        {
                                                            Size = new Size(120, 120), // Adjusted size
                                                            SizeMode = PictureBoxSizeMode.Zoom, // Maintains aspect ratio
                                                            BorderStyle = BorderStyle.None, // Removes default border
                                                            BackColor = Color.White, // Background color
                                                            Location = new Point(50, 50), // Adjust position
                                                            AllowDrop = true // Allow drag-and-drop
                                                        };


                                                        pictureBox.Paint += PictureBox_Paint;
                                                        pictureBox.Tag = "person_id:" + personid + ";" + personName;

                                                        pictureBox.Click += PictureBox_Click;

                                                        //pictureBox.MouseDown += PictureBox_MouseDown;
                                                        //pictureBox.DragEnter += PictureBox_DragEnter;
                                                        //pictureBox.DragDrop += PictureBox_DragDrop;

                                                        // Load the image into the PictureBox
                                                        try
                                                        {
                                                            using (HttpClient client = new HttpClient())
                                                            {
                                                                byte[] imageBytes = await client.GetByteArrayAsync(faces + path);

                                                                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                                                                {
                                                                    pictureBox.Image = Image.FromStream(memoryStream);
                                                                }
                                                            }
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            MessageBox.Show("Error loading image: " + ex.Message);
                                                        }

                                                        // Create label with the person's name
                                                        System.Windows.Forms.Label label = new System.Windows.Forms.Label

                                                        {
                                                            Text = personName,
                                                            AutoSize = true,
                                                            TextAlign = ContentAlignment.MiddleCenter
                                                        };

                                                        // Create container panel for PictureBox and Label
                                                        Panel panel = new Panel
                                                        {
                                                            Size = new Size(120, 150),  // Adjust for layout
                                                            Margin = new Padding(10),
                                                        };

                                                        // Position the PictureBox and Label inside the panel
                                                        pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 10);
                                                        label.Location = new Point((panel.Width - label.Width) / 2, pictureBox.Bottom + 5);

                                                        // Add PictureBox and Label to the panel
                                                        panel.Controls.Add(pictureBox);
                                                        panel.Controls.Add(label);



                                                        // Add the panel to the FlowLayoutPanel
                                                        flowLayoutPanel.Controls.Add(panel);
                                                    }
                                                    break;
                                                }
                                                catch (Exception ex)
                                                {
                                                    MessageBox.Show($"Error processing image item: {ex.Message}");
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Unexpected data format received.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else {
                        MessageBox.Show("No Face Found in the image");
                    }
                }
            }
        }


        private async void AddRoundedImageToFlowPanel(string personPath)
        {
            PictureBox picBox = new PictureBox
            {
                Size = new Size(30, 30), // Slightly larger to better show overlay
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = Image.FromStream(await Logic.LoadImageFromUrl(personPath))
            };

            picBox.Paint += PictureBox_Paint1;

            panlFaces.Controls.Add(picBox);
        }

        private void PictureBox_Paint1(object sender, PaintEventArgs e)
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

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            if (pictureBox == null) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int layerCount = 3;               // Number of folder flaps (layers)
            int flapHeight = 10;              // Height of each flap
            int tabWidth = 60;                // Width of the tab/flap
            int radius = 10;                  // Corner radius
            int spacing = 5;                  // Spacing between flaps

            // Draw background flaps (simulate folder layers)
            for (int i = layerCount - 1; i >= 1; i--)
            {
                int offset = i * spacing;
                using (GraphicsPath flapPath = new GraphicsPath())
                {
                    flapPath.StartFigure();
                    flapPath.AddArc(offset, offset, radius, radius, 180, 90);
                    flapPath.AddLine(offset + radius, offset, offset + tabWidth - radius, offset);
                    flapPath.AddArc(offset + tabWidth - radius, offset, radius, radius, 270, 90);
                    flapPath.AddLine(offset + tabWidth, offset + radius, offset + tabWidth, offset + flapHeight);
                    flapPath.AddLine(offset + tabWidth, offset + flapHeight, pictureBox.Width - radius, offset + flapHeight);
                    flapPath.AddArc(pictureBox.Width - radius, offset + flapHeight, radius, radius, 270, 90);
                    flapPath.AddLine(pictureBox.Width, offset + flapHeight + radius, pictureBox.Width, pictureBox.Height - radius);
                    flapPath.AddArc(pictureBox.Width - radius, pictureBox.Height - radius, radius, radius, 0, 90);
                    flapPath.AddLine(pictureBox.Width - radius, pictureBox.Height, offset + radius, pictureBox.Height);
                    flapPath.AddArc(offset, pictureBox.Height - radius, radius, radius, 90, 90);
                    flapPath.AddLine(offset, pictureBox.Height - radius, offset, offset + radius);
                    flapPath.CloseFigure();

                    using (Brush flapBrush = new SolidBrush(Color.FromArgb(255 - i * 30, 230, 230, 200))) // lighter layers in back
                    {
                        e.Graphics.FillPath(flapBrush, flapPath);
                    }
                    using (Pen borderPen = new Pen(Color.FromArgb(150, 100, 100, 100), 1)
                        )
                    {
                        e.Graphics.DrawPath(borderPen, flapPath);
                    }
                }
            }

            // Main folder body
            using (GraphicsPath path = new GraphicsPath())
            {
                path.StartFigure();
                path.AddArc(0, 0, radius, radius, 180, 90);
                path.AddLine(radius, 0, tabWidth - radius, 0);
                path.AddArc(tabWidth - radius, 0, radius, radius, 270, 90);
                path.AddLine(tabWidth, radius, tabWidth, flapHeight);

                path.AddLine(tabWidth, flapHeight, pictureBox.Width - radius, flapHeight);
                path.AddArc(pictureBox.Width - radius, flapHeight, radius, radius, 270, 90);

                path.AddLine(pictureBox.Width, flapHeight + radius, pictureBox.Width, pictureBox.Height - radius);
                path.AddArc(pictureBox.Width - radius, pictureBox.Height - radius, radius, radius, 0, 90);

                path.AddLine(pictureBox.Width - radius, pictureBox.Height, radius, pictureBox.Height);
                path.AddArc(0, pictureBox.Height - radius, radius, radius, 90, 90);

                path.AddLine(0, pictureBox.Height - radius, 0, radius);
                path.CloseFigure();

                pictureBox.Region = new Region(path);

                Color baseColor = Color.CornflowerBlue;
                using (SolidBrush bodyBrush = new SolidBrush(Color.FromArgb(190, baseColor.R, baseColor.G, baseColor.B)
                    //Color.LightBlue
                    ))
                {
                    e.Graphics.FillPath(bodyBrush, path);
                }

                using (Pen borderPen = new Pen(Color.Black, 2))
                {
                    e.Graphics.DrawPath(borderPen, path);
                }
            }

            // Draw image inside folder body (bottom layer)
            if (pictureBox.Image != null)
            {
                int imageX = 10;
                int imageY = flapHeight + 10;
                int imageWidth = pictureBox.Width - 20;
                int imageHeight = pictureBox.Height - flapHeight - 20;

                e.Graphics.DrawImage(pictureBox.Image, new Rectangle(imageX, imageY, imageWidth, imageHeight));
            }
            // Optional: Draw the image on the "tab" of the folder
            Image topIcon = Properties.Resources.Vector__3_; // Replace with your actual image
            int iconSize = 20;
            if (topIcon != null)
            {
                e.Graphics.DrawImage(topIcon, new Rectangle(10, 2, iconSize, iconSize)); // Adjust position as needed
            }
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox && pictureBox.Tag is string eventName)
            {
                this.Hide();
                SecondScreen form = new SecondScreen(this, eventName);
                form.Show();
            }
        }

    }
}
