using BusinessLogicLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;



namespace PhotoGallery_Desktop
{
    public partial class FirstScreen : Form
    {
        Logic logic = new Logic();
		private List<EditImageModel> selectedImages = new List<EditImageModel>();

		public FirstScreen()
        {
            InitializeComponent();
			this.AllowDrop = true;
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
        

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.ForeColor = SystemColors.Desktop;
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn != button2) // Check if it's a button and not button2
                {
                    btn.ForeColor = Color.White; // Set text color to white
                }
            }
            try
            {
                // Clear any previous controls from the panel
                panel2.Controls.Clear();

                // Initialize the logic object
                Logic logic = new Logic();

                // Fetch the data table asynchronously
                object dataTable = await logic.Group_by_person();

                // Check if the DataTable is null
                if (dataTable == null)
                {
                    MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (dataTable is List<Dictionary<string, object>> listData)
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
                                string path= personData["path"]?.ToString();
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
                                            pictureBox.Tag = "person_id:" + personid +";"+personName;

                                            pictureBox.Click += PictureBox_Click;

                                            pictureBox.MouseDown += PictureBox_MouseDown;
                                            pictureBox.DragEnter += PictureBox_DragEnter;
                                            pictureBox.DragDrop += PictureBox_DragDrop;

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


       

        private async void button3_Click(object sender, EventArgs e)
        {
            button3.ForeColor = SystemColors.Desktop;
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn != button3) // Check if it's a button and not button2
                {
                    btn.ForeColor = Color.White; // Set text color to white
                }
            }

            try
            {
                panel2.Controls.Clear();

                // Initialize the logic object
                Logic logic = new Logic();

                // Fetch the data table asynchronously
                object dataTable = await logic.Group_by_Event();

                // Check if the DataTable is null
                if (dataTable == null)
                {
                    MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (dataTable is Dictionary<string, object> dictionary)
                {
                    // Display all keys for debugging purposes
                    string keys = string.Join(", ", dictionary.Keys); // Join all keys into a comma-separated string
                    //MessageBox.Show($"Available keys: {keys}", "Keys Found", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Display all key-value pairs (optional for debugging)
                    foreach (var kvp in dictionary)
                    {
                        //MessageBox.Show($"Key: {kvp.Key}, Value: {kvp.Value}");
                        string a = kvp.Value.ToString();
                        //MessageBox.Show(a);
                        var eventsDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);

                        // Convert the dynamically deserialized object into the desired Dictionary structure
                        var finalDictionary = new Dictionary<string, List<Dictionary<string, object>>>();

                        foreach (var eventCategory in eventsDictionary)
                        {
                            // If the value is a list, we can safely convert it into the desired list of dictionaries
                            if (eventCategory.Value is Newtonsoft.Json.Linq.JArray eventArray)
                            {
                                var eventDetailsList = new List<Dictionary<string, object>>();

                                foreach (var item in eventArray)
                                {
                                    // Convert each item to a dictionary
                                    var eventDetails = item.ToObject<Dictionary<string, object>>();
                                    eventDetailsList.Add(eventDetails);
                                }

                                // Add the event category with its list of event details to the final dictionary
                                finalDictionary[eventCategory.Key] = eventDetailsList;
                            }
                        }
                        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                        {
                            Dock = DockStyle.Fill, // Fill the panel
                            AutoScroll = true,     // Enable scrolling
                            FlowDirection = FlowDirection.LeftToRight, // Arrange items vertically
                            WrapContents = true
                        };
                        panel2.Controls.Add(flowLayoutPanel);
                        //int yOffset = 10;
                        // Display the dictionary content for debugging
                        foreach (var eventCategory in finalDictionary)
                        {
                            //birthday
                            //MessageBox.Show($"Event Category: {eventCategory.Key}");


                            foreach (var eventDetails in eventCategory.Value)
                            {

                                var lastValue = eventDetails.Values.LastOrDefault();
                                if (lastValue != null)
                                {
                                    PictureBox pictureBox = new PictureBox
                                    {
                                        Size = new Size(120, 120), // Adjusted size
                                        SizeMode = PictureBoxSizeMode.Zoom, // Maintains aspect ratio
                                        BorderStyle = BorderStyle.None, // Removes default border
                                        BackColor = Color.White, // Background color
                                        Location = new Point(50, 50) // Adjust position
                                    };
                                    pictureBox.Paint += PictureBox_Paint;
                                    pictureBox.Tag = "event:"+eventCategory.Key;
                                    pictureBox.Click += PictureBox_Click;
                                    //MessageBox.Show(eventCategory.Key.GetType().ToString());
                                    string imageurl = Logic.imageurl;
                                    //loading picture in picturebox

                                    try
                                    {
                                        using (HttpClient client = new HttpClient())
                                        {
                                            // Fetch the image as a byte array
                                            byte[] imageBytes = await client.GetByteArrayAsync(imageurl + lastValue);

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
                                    System.Windows.Forms.Label label = new System.Windows.Forms.Label
                                    {
                                        Text = eventCategory.Key,
                                        AutoSize = true,
                                        TextAlign = ContentAlignment.MiddleCenter
                                    };


                                    // Create a container Panel
                                    Panel panel = new Panel
                                    {
                                        Size = new Size(120, 150), // Adjust for layout
                                        Margin = new Padding(10),

                                    };

                                    //pictureBox.Location = new Point(20, 20);
                                    //label.Location = new Point(10, pictureBox.Bottom + 5);

                                    pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 10);

                                    // Position label below PictureBox
                                    label.Location = new Point((panel.Width - label.Width) / 2, pictureBox.Bottom + 5);


                                    panel.Controls.Add(pictureBox);
                                    panel.Controls.Add(label);

                                    flowLayoutPanel.Controls.Add(panel);
                                }
                                break;
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

        private async void button4_Click(object sender, EventArgs e)
        {
            button4.ForeColor = SystemColors.Desktop;
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn != button4) // Check if it's a button and not button2
                {
                    btn.ForeColor = Color.White; // Set text color to white
                }
            }
            try
            {
                panel2.Controls.Clear();

                // Initialize the logic object
                Logic logic = new Logic();

                // Fetch the data table asynchronously
                object dataTable = await logic.Group_by_date();

                // Check if the DataTable is null
                if (dataTable == null)
                {
                    MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (dataTable is Dictionary<string, object> dictionary)
                {
                    // Display all keys for debugging purposes
                    string keys = string.Join(", ", dictionary.Keys); // Join all keys into a comma-separated string
                    //MessageBox.Show($"Available keys: {keys}", "Keys Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var finalDictionary = new Dictionary<string, List<Dictionary<string, object>>>();

                    // Display all key-value pairs (optional for debugging)
                    foreach (var kvp in dictionary)
                    {
                        //MessageBox.Show($"Value: {kvp.Value},Value Type: {kvp.Value.GetType()}\"");
                        if (kvp.Value is Newtonsoft.Json.Linq.JArray eventArray)
                        {
                            var dateDetailsList = new List<Dictionary<string, object>>();

                            foreach (var item in eventArray)
                            {
                                // Convert each item to a dictionary
                                var eventDetails = item.ToObject<Dictionary<string, object>>();
                                dateDetailsList.Add(eventDetails);
                            }

                            // Add the event category with its list of event details to the final dictionary
                            finalDictionary[kvp.Key] = dateDetailsList;
                        }
                    }
                    //new
                    FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                    {
                        Dock = DockStyle.Fill, // Fill the panel
                        AutoScroll = true,     // Enable scrolling
                        FlowDirection = FlowDirection.LeftToRight, // Arrange items vertically
                        WrapContents = true   // Prevent wrapping to a new column
                    };
                    panel2.Controls.Add(flowLayoutPanel);
                    //int yOffset = 10;
                    // Display the dictionary content for debugging
                    foreach (var eventCategory in finalDictionary)
                    {
                        //birthday
                        //MessageBox.Show($"Event Category: {eventCategory.Key}");


                        foreach (var eventDetails in eventCategory.Value)
                        {

                            var lastValue = eventDetails.Values.LastOrDefault();
                            var firstValue = eventDetails.Values.FirstOrDefault();
                            if (lastValue != null)
                            {
                                PictureBox pictureBox = new PictureBox
                                {
                                    Size = new Size(120, 120), // Adjusted size
                                    SizeMode = PictureBoxSizeMode.Zoom, // Maintains aspect ratio
                                    BorderStyle = BorderStyle.None, // Removes default border
                                    BackColor = Color.White, // Background color
                                    Location = new Point(50, 50) // Adjust position
                                };
                                pictureBox.Paint += PictureBox_Paint;
                                pictureBox.Tag = "capture_date:"+firstValue.ToString();
                                pictureBox.Click += PictureBox_Click;
                                //MessageBox.Show(eventCategory.Key.GetType().ToString());
                                string imageurl = Logic.imageurl;
                                //loading picture in picturebox

                                try
                                {
                                    using (HttpClient client = new HttpClient())
                                    {
                                        // Fetch the image as a byte array
                                        byte[] imageBytes = await client.GetByteArrayAsync(imageurl + lastValue);

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
                                System.Windows.Forms.Label label = new System.Windows.Forms.Label
                                {
                                    Text = firstValue.ToString(),
                                    AutoSize = true,
                                    TextAlign = ContentAlignment.MiddleCenter
                                };


                                // Create a container Panel
                                Panel panel = new Panel
                                {
                                    Size = new Size(120, 150), // Adjust for layout
                                    Margin = new Padding(10),

                                };

                                //pictureBox.Location = new Point(20, 20);
                                //label.Location = new Point(10, pictureBox.Bottom + 5);

                                pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 10);

                                // Position label below PictureBox
                                label.Location = new Point((panel.Width - label.Width) / 2, pictureBox.Bottom + 5);


                                panel.Controls.Add(pictureBox);
                                panel.Controls.Add(label);

                                //panel.Location = new Point(yOffset, 10);
                                //yOffset += panel.Height + 10;
                                // Add panel to FlowLayoutPanel
                                flowLayoutPanel.Controls.Add(panel);
                            }
                            break;
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

		//private void PictureBox_MouseDown(object sender, MouseEventArgs e)
		//{
		//	PictureBox pb = sender as PictureBox;
		//	if (pb != null && pb.Image != null)
		//	{
		//		pb.DoDragDrop(pb, DragDropEffects.Move);
		//	}
		//}

		private void PictureBox_MouseDown(object sender, MouseEventArgs e)
		{
			PictureBox pb = sender as PictureBox;
			if (pb != null && pb.Image != null)
			{
				Task.Delay(150).ContinueWith(_ =>
				{
					if (pb.IsHandleCreated)
						pb.Invoke(new Action(() => pb.DoDragDrop(pb, DragDropEffects.Move)));
				});
			}
		}


		private void PictureBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(PictureBox)))
			{
				e.Effect = DragDropEffects.Move;
			}
		}

		private void PictureBox_DragDrop(object sender, DragEventArgs e)
		{
			PictureBox targetBox = sender as PictureBox;
			PictureBox draggedBox = (PictureBox)e.Data.GetData(typeof(PictureBox));

			if (targetBox == null || draggedBox == null || targetBox == draggedBox)
				return;

			// Show confirmation popup
			DialogResult result = MessageBox.Show("Are these images of the same person?",
												  "Confirm Merge",
												  MessageBoxButtons.YesNo,
												  MessageBoxIcon.Question);

			if (result == DialogResult.Yes)
			{
				// Extract person IDs from Tags
				string tag1 = draggedBox.Tag?.ToString();
				string tag2 = targetBox.Tag?.ToString();

				if (!string.IsNullOrEmpty(tag1) && !string.IsNullOrEmpty(tag2))
				{
					int personId1 = ExtractPersonId(tag1);
					int personId2 = ExtractPersonId(tag2);

					// Call Merge Action
					_ = Logic.SaveLinkDataAsync(personId1, personId2);
				}
			}
		}

		private int ExtractPersonId(string tag)
		{
			string[] parts = tag.Split(';');
			foreach (var part in parts)
			{
				if (part.StartsWith("person_id:"))
				{
					return int.Parse(part.Split(':')[1]);
				}
			}
			return -1; // Return -1 if ID not found
		}
		private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            //PictureBox pictureBox = sender as PictureBox;
            //if (pictureBox == null) return;

            //// Create a GraphicsPath to define rounded corners
            //using (GraphicsPath path = new GraphicsPath())
            //{
            //    int radius = 20; // Adjust corner radius
            //    path.AddArc(0, 0, radius, radius, 180, 90);
            //    path.AddArc(pictureBox.Width - radius, 0, radius, radius, 270, 90);
            //    path.AddArc(pictureBox.Width - radius, pictureBox.Height - radius, radius, radius, 0, 90);
            //    path.AddArc(0, pictureBox.Height - radius, radius, radius, 90, 90);
            //    path.CloseFigure();
            //    pictureBox.Region = new Region(path); // Apply rounded region

            //    // Draw the black rounded border
            //    using (Pen borderPen = new Pen(Color.Black, 4)) // Black border with width 4
            //    {
            //        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Smooths edges
            //        e.Graphics.DrawPath(borderPen, path); // Draw the border along the path
            //    }
            //}
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
                    using (Pen borderPen = new Pen(Color.FromArgb(150, 100, 100, 100), 1))
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

                using (SolidBrush bodyBrush = new SolidBrush(Color.LightBlue))
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
        private async void button5_Click(object sender, EventArgs e)
        {
            button5.ForeColor = SystemColors.Desktop;
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn != button5) // Check if it's a button and not button2
                {
                    btn.ForeColor = Color.White; // Set text color to white
                }
            }
            try
            {
                panel2.Controls.Clear();

                // Initialize the logic object
                Logic logic = new Logic();

                // Fetch the data table asynchronously
                object dataTable = await logic.Group_by_Location();

                // Check if the DataTable is null
                if (dataTable == null)
                {
                    MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (dataTable is Dictionary<string, object> dictionary)
                {
                    // Display all keys for debugging purposes
                    string keys = string.Join(", ", dictionary.Keys); // Join all keys into a comma-separated string
                    //MessageBox.Show($"Available keys: {keys}", "Keys Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var finalDictionary = new Dictionary<string, List<Dictionary<string, object>>>();

                    // Display all key-value pairs (optional for debugging)
                    foreach (var kvp in dictionary)
                    {
                        // MessageBox.Show($"KEY:{kvp.Key},Value: {kvp.Value},Value Type: {kvp.Value.GetType().Name}\"");
                        if (kvp.Value is JObject eventArray)
                        {
                            var dateDetailsList = new List<Dictionary<string, object>>();

                            // Iterate over the properties of the JObject
                            var imagesArray = eventArray["images"] as JArray;

                            if (imagesArray != null)
                            {
                                // Iterate over each item in the "images" array
                                foreach (var item in imagesArray)
                                {
                                    try
                                    {
                                        // Convert each item (image) to a dictionary
                                        var eventDetails = item.ToObject<Dictionary<string, object>>();

                                        // If conversion is successful, add to the list
                                        if (eventDetails != null)
                                        {
                                            dateDetailsList.Add(eventDetails);
                                        }
                                        else
                                        {
                                            Console.WriteLine("Failed to convert item to dictionary.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Error processing item: {ex.Message}");
                                    }
                                }

                                // Add the "images" list to the final dictionary
                                finalDictionary[kvp.Key] = dateDetailsList;
                            }
                            else
                            {
                                Console.WriteLine("No 'images' array found.");
                            }

                            // Add the event category with its list of event details to the final dictionary
                            finalDictionary[kvp.Key] = dateDetailsList;
                        }
                        //new
                        FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                        {
                            Dock = DockStyle.Fill,
                            AutoScroll = true
                        };
                        panel2.Controls.Add(flowLayoutPanel);
                        //int yOffset = 10;
                        // Display the dictionary content for debugging
                        foreach (var eventCategory in finalDictionary)
                        {
                            //birthday
                            //MessageBox.Show($"Event Category: {eventCategory.Key}");


                            foreach (var eventDetails in eventCategory.Value)
                            {

                                var lastValue = eventDetails.Values.LastOrDefault();
                                var firstValue = eventDetails.Values.FirstOrDefault();
                                if (lastValue != null)
                                {
                                    PictureBox pictureBox = new PictureBox
                                    {
                                        Size = new Size(120, 120), // Adjusted size
                                        SizeMode = PictureBoxSizeMode.Zoom, // Maintains aspect ratio
                                        BorderStyle = BorderStyle.None, // Removes default border
                                        BackColor = Color.White, // Background color
                                        Location = new Point(50, 50) // Adjust position
                                    };
                                    pictureBox.Paint += PictureBox_Paint;
                                    pictureBox.Tag = "location:"+eventCategory.Key;
                                    pictureBox.Click += PictureBox_Click;
                                    //MessageBox.Show(eventCategory.Key.GetType().ToString());
                                    string imageurl = Logic.imageurl;
                                    //loading picture in picturebox

                                    try
                                    {
                                        using (HttpClient client = new HttpClient())
                                        {
                                            // Fetch the image as a byte array
                                            byte[] imageBytes = await client.GetByteArrayAsync(imageurl + lastValue);

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
                                    System.Windows.Forms.Label label = new System.Windows.Forms.Label
                                    {
                                        Text = eventCategory.Key,
                                        AutoSize = true,
                                        TextAlign = ContentAlignment.MiddleCenter
                                    };


                                    // Create a container Panel
                                    Panel panel = new Panel
                                    {
                                        Size = new Size(120, 150), // Adjust for layout
                                        Margin = new Padding(10),

                                    };

                                    //pictureBox.Location = new Point(20, 20);
                                    //label.Location = new Point(10, pictureBox.Bottom + 5);

                                    pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 10);

                                    // Position label below PictureBox
                                    label.Location = new Point((panel.Width - label.Width) / 2, pictureBox.Bottom + 5);


                                    panel.Controls.Add(pictureBox);
                                    panel.Controls.Add(label);

                                    flowLayoutPanel.Controls.Add(panel);
                                }
                                break;
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

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.ForeColor = SystemColors.Desktop;
            foreach (Control control in this.Controls)
            {
                if (control is Button btn && btn != button1) // Check if it's a button and not button2
                {
                    btn.ForeColor = Color.White; // Set text color to white
                }
            }
            try
            {
                //label1.Text = "Label";
                // Clear previous controls from panel
                panel2.Controls.Clear();

                // Initialize the logic object
                Logic logic = new Logic();

                // Fetch the unlabelled images asynchronously
                object responseData = await logic.UnlabelledImages();
                JObject jsonResponse = JObject.Parse(responseData.ToString());

                // Check if "unedited_images" exists and is a valid JArray
                if (jsonResponse.ContainsKey("unedited_images") && jsonResponse["unedited_images"] is JArray imagesArray)
                {
                    // Deserialize the JArray into a List<EditImageModel>
                    var imagesList = imagesArray.ToObject<List<EditImageModel>>();

                    if (imagesList != null && imagesList.Count > 0)
                    {
                        // Create a FlowLayoutPanel to hold PictureBoxes
                        FlowLayoutPanel flowLayoutPanel1 = new FlowLayoutPanel
                        {
                            Dock = DockStyle.Fill, // Fill the panel
                            AutoScroll = true,     // Enable scrolling
                            FlowDirection = FlowDirection.LeftToRight, // Arrange items horizontally
                            WrapContents = true    // Wrap contents if the panel gets full
                        };
                        panel2.Controls.Add(flowLayoutPanel1);  // Add the FlowLayoutPanel to the parent panel

                        // Loop through the list of images and display each image in a PictureBox
                        foreach (var imgData in imagesList)
                        {
                            // Ensure the image path is not null or empty
                            if (!string.IsNullOrEmpty(imgData.path))
                            {
                                // Create a PictureBox for each image
                                PictureBox pictureBox = new PictureBox
                                {
                                    Width = 150,
                                    Height = 150,
                                    SizeMode = PictureBoxSizeMode.Zoom,
                                    BorderStyle = BorderStyle.FixedSingle,
                                   
                                };
								//pictureBox.Tag = "Label:" + imgData;
								string jsonData = JsonConvert.SerializeObject(imgData);
								pictureBox.Tag = "Label:" + jsonData;

								string imageurl = Logic.imageurl;

                                try
                                {
                                    // Fetch the image as a byte array
                                    using (HttpClient client = new HttpClient())
                                    {
                                        byte[] imageBytes = await client.GetByteArrayAsync(imageurl+imgData.path);
                                        //byte[] imageBytes = File.ReadAllBytes(imgData.path);

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

                                // Create a panel to hold the PictureBox (for layout)
                                Panel panel = new Panel
                                {
                                    Size = new Size(120, 150), // Adjust for layout
                                    Margin = new Padding(10),  // Add margin for spacing
                                };

                                // Center the PictureBox within the Panel
                                pictureBox.Location = new Point((panel.Width - pictureBox.Width) / 2, 12);
                                panel.Controls.Add(pictureBox);  // Add PictureBox to the panel

                                // Add the panel to the FlowLayoutPanel
                                flowLayoutPanel1.Controls.Add(panel);
                                //new click event
                                pictureBox.Click += (s, args) =>
                                {
                                    try
                                    {
                                        //if (s is PictureBox pb && pb.Tag is EditImageModel clickedImage)
                                        //{
                                            //int imageId = clickedImage.id; // Extract ID
                                            EDDB eddb = new EDDB(this, imgData.id); // Pass ID to EDDB form
                                            this.Hide();
                                            eddb.Show();
                                        //}
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Error opening EDDB form: " + ex.Message);
                                    }
                                };
                                // Handle the click event for the PictureBox (optional)
                                //pictureBox.Click += PictureBox_Click;
                            }
                        }
                    }
                }
                else
                {
                    // Handle unexpected data format
                    MessageBox.Show("Unexpected data format received.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FirstScreen_Load(object sender, EventArgs e)
        {
			bulkEditButton.Visible = false;
            button1_Click(sender, e);
		}

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(SettingBtn, new System.Drawing.Point(-30, SettingBtn.Height));

        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
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
                            checkBox.CheckedChanged += CheckBox_CheckedChanged;

                            imagePanel.Controls.Add(checkBox);
                        }
                    }
                }
            }
        }

		private void CheckBox_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null) return;

			Panel imagePanel = checkBox.Parent as Panel;
			if (imagePanel == null) return;

			PictureBox pictureBox = imagePanel.Controls.OfType<PictureBox>().FirstOrDefault();
			if (pictureBox != null && pictureBox.Tag is string tagString && tagString.StartsWith("Label:"))
			{
				string jsonData = tagString.Substring(6);
				EditImageModel imgData = JsonConvert.DeserializeObject<EditImageModel>(jsonData);
				if (checkBox.Checked)
				{
					selectedImages.Add(imgData);
				}
				else
				{
					var img = selectedImages.Where(i => i.id == imgData.id).FirstOrDefault();
					selectedImages.Remove(img);
				}
			}

			// Show "Bulk Edit" button if images are selected
			if (bulkEditButton == null)
			{
				bulkEditButton = new Button
				{
					Text = "Bulk Edit",
					AutoSize = true,
					Location = new Point(panel2.Width / 2 - 50, panel2.Height - 50),
					Visible = false
				};

				//bulkEditButton.Click += bulkEditButton_Click;
				panel2.Controls.Add(bulkEditButton);
			}

			bulkEditButton.Visible = selectedImages.Count > 0;
		}


		private void syncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SyncImages sync_images = new SyncImages();
            this.Hide();
            sync_images.Show();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SearchImage search_images = new SearchImage(this);
            this.Hide();
            search_images.Show();

        }

		private void bulkEditButton_Click(object sender, EventArgs e)
		{
			if (selectedImages.Count > 0)
			{
				AddBulkDataForm addBulkDataForm = new AddBulkDataForm(selectedImages);
				addBulkDataForm.ShowDialog();
			}
		}

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageGallery imagegallery = new ImageGallery(this);
            imagegallery.Show();
            this.Hide();
        }
    }
}
