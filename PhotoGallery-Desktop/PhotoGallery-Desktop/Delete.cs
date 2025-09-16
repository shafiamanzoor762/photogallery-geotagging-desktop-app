using BusinessLogicLayer;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer2;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace PhotoGallery_Desktop
{
    public partial class Delete : Form
    {


            private int id;
            private Form previousForm;

            public Delete(Form previousForm, int id)
            {
                InitializeComponent();
                this.id = id;
                this.previousForm = previousForm;
            }
            private void radioButton1_CheckedChanged(object sender, EventArgs e)
            {
                if (radioButton1.Checked)
                {
                    // Update the UI when "Delete Image" is selected
                    label2.Text = "You have selected to delete image."; // Update label text

                    // Optionally, change the picture or any other visual element to indicate the action
                    // pictureBox1.Image = Properties.Resources.DeleteImageIcon; // Assuming you have an icon or image for DeleteImage (this is just an example)

                    // You can also enable/disable other controls if needed
                    btnokay.Enabled = true; // Enable the Okay button when the option is selected
                }


            }


            //private async void Delete_Load(object sender, EventArgs e)
            //{
            //    await LoadImageByIdAsync(id);
            //}
            //private async Task LoadImageByIdAsync(int id)
            //{
            //    Logic logic = new Logic();
            //    var imageDetails = await logic.FetchImageDetails(id);

            //    if (imageDetails != null)
            //    {
            //        string fullUrl = "http://127.0.0.1:5000/" + imageDetails.path;

            //        try
            //        {
            //            // Using HttpClient inside a 'using' block for simplicity, though reusing HttpClient is better
            //            using (HttpClient client = new HttpClient())
            //            {
            //                byte[] imageBytes = await client.GetByteArrayAsync(fullUrl);

            //                using (MemoryStream ms = new MemoryStream(imageBytes))
            //                {
            //                    pictureBox1.Image = Image.FromStream(ms);
            //                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // Displaying a message if there's an error loading the image
            //            MessageBox.Show($"Failed to load image: {ex.Message}");
            //        }
            //    }
            //    else
            //    {
            //        // If the image details are not found, show an error message
            //        MessageBox.Show("Image details not found.");
            //    }
            //}
            private async Task LoadImageByIdAsync(int id)
            {
                Logic logic = new Logic();
                var imageDetails = await logic.FetchImageDetails(id);

                if (imageDetails != null)
                {
                    try
                    {
                        string path = imageDetails.path;

                        if (File.Exists(path))
                        {
                            byte[] imageBytes = File.ReadAllBytes(path);
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                pictureBox1.Image = Image.FromStream(ms);
                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Image file not found at: " + path);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to load image: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Image details not found.");
                }
            }
            //private void radioButton1_CheckedChanged(object sender, EventArgs e)
            //{
            //    if (radioButton1.Checked)
            //    {
            //        // Optional: Show or hide controls, enable/disable buttons, etc.
            //        Console.WriteLine("Delete image option selected.");
            //    }
            //}
            private async void Delete_Load(object sender, EventArgs e)
            {
                await LoadImageByIdAsync(this.id);
            }


            private void radioButton2_CheckedChanged(object sender, EventArgs e)
            {
                if (radioButton2.Checked)
                {
                    // Update the UI when "Delete Metadata" is selected
                    label2.Text = "You have selected to delete the metadata."; // Update label text

                    // Optionally change the picture to a different icon if needed
                    //pictureBox1.Image = Properties.Resources.DeleteMetadataIcon; // Example: replace with an actual image resource

                    // Enable the okay button
                    btnokay.Enabled = true;
                }
            }
            private void btnCancel_Click(object sender, EventArgs e)
            {
                this.Close();
                previousForm.Show();
            }

            private async void btnokay_Click(object sender, EventArgs e)
            {

                Api api = new Api(); // Instantiate Api class

                // Check if the user selected "Delete Image"
                if (radioButton1.Checked)
                {
                    // User wants to delete the image
                    MessageBox.Show("Deleting image...");
                    bool result = await api.DeleteImage(id.ToString());
                    MessageBox.Show(result ? "Image deleted successfully." : "Failed to delete the image.");
                }
                else if (radioButton2.Checked)
                {
                    // Handle metadata deletion
                }
                else
                {
                    // If no radio button is selected, show an error
                    MessageBox.Show("Please select an option to delete.");
                }
            }

            private void pictureBox4_Click(object sender, EventArgs e)
            {
                this.Close(); // Close current form
                previousForm.Show();
            }
        }
    }

