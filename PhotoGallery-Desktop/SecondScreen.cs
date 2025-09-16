using BusinessLogicLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PhotoGallery_Desktop
{
    public partial class SecondScreen : Form
    {
        private Form previousForm;
        private List<EditImageModel> selectedImages = new List<EditImageModel>();


        private string[] parts;
        public string secondPart;
        public string firstPart;

        public object senderID;
        public EventArgs e_id;

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox pictureBox && pictureBox.Tag is Dictionary<string, object> imgData)
            {
                
                   
                   
                }
            }
        

        public SecondScreen(Form previousForm, string name)
        {
            InitializeComponent();
            this.previousForm = previousForm;


            parts = name.Split(';');

            if (parts.Length == 2)
            {
                firstPart = parts[0]; // "person_id:2"
                //MessageBox.Show(firstPart);
                string secondPart = parts[1]; // "amna"

                label1.Text = secondPart;

            }
            else
            {
                firstPart = parts[0];
                string[] nameparts = firstPart.Split(':');
               
                label1.Text = nameparts[1];
                //MessageBox.Show(firstPart);
            }
        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); // Close current form
            previousForm.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private async void SecondScreen_Load_1(object sender, EventArgs e)
        {
            senderID = sender;
            e_id = e;

            Logic logic = new Logic();

            string[] kvp = firstPart.Split(':');
            if (kvp[0] == "Label")
            {

                object a = kvp[1];
                MessageBox.Show(a.ToString());
                label1.Text = "Label";
            }
            else
            {
                object responseData = await logic.LoadImages(kvp[0], kvp[1]);

           


                if (responseData == null)
                {
                    MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    // Ensure the response is in a format we expect (List<EditImageModel>)
                    if (responseData is List<EditImageModel> imagesList && imagesList.Count > 0)
                    {
                        // Create a FlowLayoutPanel to hold PictureBoxes
                        FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel
                        {
                            Dock = DockStyle.Fill, // Fill the panel
                            AutoScroll = true,     // Enable scrolling
                            FlowDirection = FlowDirection.LeftToRight, // Arrange items vertically
                            WrapContents = true
                        };
                        panel2.Controls.Add(flowLayoutPanel1);

                        // Clear any previous controls
                        //flowLayoutPanel1.Controls.Clear();

                        // Loop through the list of images and display each image in a PictureBox
                        foreach (var imgData in imagesList)
                        {
                            if (!string.IsNullOrEmpty(imgData.path))
                            {
                                PictureBox pictureBox = new PictureBox
                                {
                                    Width = 150,
                                    Height = 150,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    BorderStyle = BorderStyle.FixedSingle,
                                    Tag = imgData // Store the image details in the Tag property
                                };
                                string imageurl = Logic.imageurl;
                                try
                                {
                                    using (HttpClient client = new HttpClient())
                                    {
                                        // Fetch the image as a byte array
                                        byte[] imageBytes = await client.GetByteArrayAsync(imageurl + imgData.path);

                                        // Load the image from the byte array using a MemoryStream
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
                                Panel panel = new Panel
                                {
                                    Size = new Size(120, 150), // Adjust for layout
                                    Margin = new Padding(10),

                                };


                                pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 10);
                                panel.Controls.Add(pictureBox);



                                panel2.Controls.Add(panel);


                                pictureBox.Click += (s, args) =>
                                {
                                    try
                                    {
                                        if (s is PictureBox pb && pb.Tag is EditImageModel clickedImage)
                                        {
                                            int imageId = clickedImage.id; // Extract ID
                                            EDDB eddb = new EDDB(this, imageId); // Pass ID to EDDB form
                                            this.Hide();
                                            eddb.Show();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Error opening EDDB form: " + ex.Message);
                                    }
                                };

                                //pictureBox.Click += PictureBox_Click;


                                flowLayoutPanel1.Controls.Add(panel);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No images found in the response.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error parsing the API response: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void CheckBox_CheckedChangedforMoveImages(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            Panel imagePanel = checkBox.Parent as Panel;
            if (imagePanel == null) return;

            PictureBox pictureBox = imagePanel.Controls.OfType<PictureBox>().FirstOrDefault();
            if (pictureBox != null && pictureBox.Tag is EditImageModel imgData)
            {
                // Agar yeh kisi label wali image hai
                if (imgData!= null) // ya aisi koi property check karo
                {
                    if (checkBox.Checked)
                    {
                        selectedImages.Add(imgData);
                    }
                    else
                    {
                        var img = selectedImages.FirstOrDefault(i => i.id == imgData.id);
                        selectedImages.Remove(img);
                    }
                }
            }



            // Show "Bulk Edit" button if images are selected


        }

        private void moveImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control control in panel2.Controls)
            {
                if (control is FlowLayoutPanel flowLayoutPanel)
                {
                    foreach (Control panel in flowLayoutPanel.Controls)
                    {
                        if (panel is Panel imagePanel)
                        {
                            CheckBox checkBox = new CheckBox
                            {
                                Location = new Point(2, 0),
                                AutoSize = true
                            };

                            // Checkbox event to show/hide "Bulk Edit" button
                            checkBox.CheckedChanged += CheckBox_CheckedChangedforMoveImages;

                            imagePanel.Controls.Add(checkBox);
                        }
                    }
                }
            }
        }

        private void SettingBtn_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(SettingBtn, new System.Drawing.Point(-30, SettingBtn.Height));

        }

        private void sharebutton_Click(object sender, EventArgs e)
        {

            List<Person> allPersons = new List<Person>();

            foreach (var img in selectedImages)
            {
                if (img.persons != null)
                {
                    foreach (var person in img.persons)
                    {
                        allPersons.Add(person); // Adds duplicates too
                    }
                }
            }

            //"person_id:2"
            var parts = firstPart.Split(':');

            string sourceId = parts[1];


            SelectPersonForm sel_person = new SelectPersonForm(this, sourceId,allPersons  );
            sel_person.Show();
            this.Hide();
        }
    }
}