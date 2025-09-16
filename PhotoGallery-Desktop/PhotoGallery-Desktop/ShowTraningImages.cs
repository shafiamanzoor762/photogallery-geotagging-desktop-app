using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoGallery_Desktop
{
	public partial class ShowTraningImages : Form
	{
		private Form previousForm;
		private Person people= new Person();

		public ShowTraningImages(Form previousForm, Person people)
		{
			InitializeComponent();
			this.previousForm = previousForm;
			this.people = people;
		}

		private async void ShowTraningImages_Load(object sender, EventArgs e)
		{
					object responseData = await Logic.GetPersonListAsync(people.id);




					if (responseData == null)
					{
						MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}

					try
					{
						// Ensure the response is in a format we expect (List<EditImageModel>)
						if (responseData is List<Person> imagesList && imagesList.Count > 0)
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

									try
									{
										using (HttpClient client = new HttpClient())
										{
											// Fetch the image as a byte array
											byte[] imageBytes = await client.GetByteArrayAsync(imgData.path);

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

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			this.Close(); // Close current form
			previousForm.Show();
		}
	}
}
