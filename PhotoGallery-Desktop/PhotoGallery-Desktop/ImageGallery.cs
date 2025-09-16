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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace PhotoGallery_Desktop
{

    public partial class ImageGallery : Form
    {
        Logic logic;
        private Form previousForm;
        //rafia
        private string[] selectedPaths;
        private FlowLayoutPanel recentPanel;
        private Stack<string> folderHistory = new Stack<string>();
        private string currentPath;


        //rafia

        public ImageGallery(Form previousForm)
        {
            logic = new Logic();
            InitializeComponent();
            AllowDrop = true;
            DragEnter += new DragEventHandler(Form1_DragEnter);
            DragDrop += new DragEventHandler(Form1_DragDrop);
            folderHistory = new Stack<string>();
            this.previousForm = previousForm;
            SetupUI();


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
                using (Pen borderPen = new Pen(Color.Black, 4)) // Black border with width 4
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Smooths edges
                    e.Graphics.DrawPath(borderPen, path); // Draw the border along the path
                }
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
        public void ShowImages(string[] imagePaths)
        {

            this.Text = "Image Gallery";
            //this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Ensure that 'showimagegallery' is used instead of creating a new panel
            showimagegallery.AutoScroll = true;


            foreach (string imagePath in imagePaths)
            {
                PictureBox picBox = new PictureBox
                {
                    Image = Image.FromFile(imagePath),
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Width = 100,
                    Height = 100,
                    Margin = new Padding(10)
                };
                showimagegallery.Controls.Add(picBox); // Add to the existing panel
            }
        }


        [STAThread]
        private async void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog
            {
                Description = "Select a Folder",
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) // Default to Desktop
            };

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolder = folderDialog.SelectedPath;
                imagespath.Text = selectedFolder;

                // 🔥 Save the selected folder to backend (.env)
                await logic.SaveDirectoryToBackend(selectedFolder);

                // 🔥 Get all image files from the selected folder and subfolders
                string[] imageFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                ShowImages(imageFiles);

                // 🔥 Send images to backend with relative paths
                await logic.ProcessImagesAsync(imageFiles, selectedFolder);
            }
        }


        private void SettingBtn_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); // Close current form
            previousForm.Show();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ImageGallery_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Filter by Person");
            comboBox1.Items.Add("Filter by Event");
            comboBox1.Items.Add("Filter by Date");

            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilter = comboBox1.SelectedItem.ToString();
            //label3.Text = selectedFilter;

            if (selectedFilter == "Filter by Person")
            {
                Person();
            }
            else if (selectedFilter == "Filter by Event")
            {
                Event();
            }
            else
            {
                Date();
            }

        }
        public async void Person()
        {
            // button2.ForeColor = SystemColors.Desktop;
            //foreach (Control control in this.Controls)
            //{
            //    if (control is Button btn && btn != button2) // Check if it's a button and not button2
            //    {
            //        btn.ForeColor = Color.White; // Set text color to white
            //    }
            //}
            try
            {
                // Clear any previous controls from the panel
                showimagegallery.Controls.Clear();

                // Initialize the
                // object
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
                    this.Controls.Add(flowLayoutPanel);

                    // Iterate over the listData
                    int yOffset = 10;

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

                                            // Set the panel's position in the flow layout
                                            panel.Location = new Point(yOffset, 10);
                                            yOffset += panel.Height + 10;

                                            // Add the panel to the FlowLayoutPanel
                                            showimagegallery.Controls.Add(panel);
                                        }
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
        public async void Event()
        {
            //    button3.ForeColor = SystemColors.Desktop;
            //    foreach (Control control in this.Controls)
            //    {
            //        if (control is Button btn && btn != button3) // Check if it's a button and not button2
            //        {
            //            btn.ForeColor = Color.White; // Set text color to white
            //        }
            //    }

            try
            {
                showimagegallery.Controls.Clear();

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
                        this.Controls.Add(flowLayoutPanel);
                        int yOffset = 10;
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
                                    pictureBox.Tag = "event:" + eventCategory.Key;
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

                                    panel.Location = new Point(yOffset, 10);
                                    yOffset += panel.Height + 10;
                                    // Add panel to FlowLayoutPanel
                                    showimagegallery.Controls.Add(panel);
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
        public async void Date()
        {
            //button4.ForeColor = SystemColors.Desktop;
            //foreach (Control control in this.Controls)
            //{
            //    if (control is Button btn && btn != button4) // Check if it's a button and not button2
            //    {
            //        btn.ForeColor = Color.White; // Set text color to white
            //    }
            //}
            try
            {
                showimagegallery.Controls.Clear();

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
                    showimagegallery.Controls.Add(flowLayoutPanel);
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
                                pictureBox.Tag = "capture_date:" + firstValue.ToString();
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
                                showimagegallery.Controls.Add(panel);
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
        private void label3_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        ///////////////////////////RAFIA WORK/////////////////////////////////////////////////////////////////////////

        private void SetupUI()
        {
            imagespath.ReadOnly = true;
            imagespath.Height = 25;

            showimagegallery.AutoScroll = true;
            showimagegallery.WrapContents = true; // Only if it's a FlowLayoutPanel
            showimagegallery.BringToFront();
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Folder";
                fbd.ShowNewFolderButton = false;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    selectedPaths = new string[] { fbd.SelectedPath };
                    ShowContents(selectedPaths);
                    imagespath.Text = fbd.SelectedPath;
                }
            }
        }

        private async void ShowContents(string[] paths)
        {
            if (paths != null && paths.Length > 0)
            {
                imagespath.Text = paths[0];

                if (folderHistory.Count == 0 || folderHistory.Peek() != paths[0])
                {
                    folderHistory.Push(paths[0]);
                }
            }

            showimagegallery.Controls.Clear();
            List<string> allImageFiles = new List<string>();

            foreach (string rootPath in paths)
            {
                await TraverseAndDisplayFoldersAsync(rootPath, allImageFiles);

                // Show images from root folder
                foreach (string file in Directory.GetFiles(rootPath).Where(IsImageFile))
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        Image = Image.FromFile(file),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Width = 100,
                        Height = 100,
                        Margin = new Padding(5),
                        Tag = file
                    };

                    pictureBox.Click += (s, e) => ShowFullImage((string)((PictureBox)s).Tag);

                    showimagegallery.Controls.Add(pictureBox);
                    allImageFiles.Add(file);
                }
            }

            await logic.ProcessImagesAsync(allImageFiles.ToArray(), imagespath.Text);
        }

        private async Task TraverseAndDisplayFoldersAsync(string rootFolder, List<string> allImageFiles)
        {
            // Get and display subfolders first
            foreach (string folder in Directory.GetDirectories(rootFolder))
            {
                // Check if the folder or any of its subfolders contain image files
                bool hasImages = Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories)
                                          .Any(IsImageFile);

                if (hasImages)
                {
                    FlowLayoutPanel folderItem = new FlowLayoutPanel
                    {
                        Width = 100,
                        Height = 120,
                        FlowDirection = FlowDirection.TopDown,
                        Margin = new Padding(10),
                        Tag = folder,
                        Cursor = Cursors.Hand
                    };

                    PictureBox folderIcon = new PictureBox
                    {
                        Image = SystemIcons.WinLogo.ToBitmap(),
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Width = 80,
                        Height = 80
                    };

                    Label folderLabel = new Label
                    {
                        Text = Path.GetFileName(folder),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Width = 100,
                        AutoEllipsis = true
                    };

                    folderItem.Controls.Add(folderIcon);
                    folderItem.Controls.Add(folderLabel);

                    EventHandler openFolder = async (s, e) =>
                    {
                        selectedPaths = new string[] { folder };
                        ShowContents(selectedPaths);
                    };

                    folderItem.Click += openFolder;
                    folderIcon.Click += openFolder;
                    folderLabel.Click += openFolder;

                    showimagegallery.Controls.Add(folderItem);
                }

                // Recursively go into subfolders (to add them to UI if they contain images)
                await TraverseAndDisplayFoldersAsync(folder, allImageFiles);
            }
        }

        private bool IsImageFile(string filePath)
        {
            string[] extensions = { ".jpg", ".jpeg", ".png", ".bmp" };
            return extensions.Contains(Path.GetExtension(filePath).ToLower());
        }

        private void ShowFullImage(string imagePath)
        {
            Form fullImageForm = new Form
            {
                Text = "Full Image View",
                Width = 800,
                Height = 600
            };

            PictureBox pictureBox = new PictureBox
            {
                Image = Image.FromFile(imagePath),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            fullImageForm.Controls.Add(pictureBox);
            fullImageForm.Show();
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] droppedPaths = (string[])e.Data.GetData(DataFormats.FileDrop);
            selectedPaths = droppedPaths.Where(Directory.Exists).ToArray();

            if (selectedPaths.Length > 0)
            {
                ShowContents(selectedPaths);
            }
            else
            {
                MessageBox.Show("No valid folders found.", "Error");
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (folderHistory.Count > 1)
            {
                folderHistory.Pop();
                string parentFolder = folderHistory.Peek();
                ShowContents(new string[] { parentFolder });
            }
            else
            {
                MessageBox.Show("You are already at the root folder.");
            }
        }

        private void imagespath_TextChanged_1(object sender, EventArgs e)
        {
            // Empty handler (may be for future use or debugging)
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            // Empty handler (may be for future use or debugging)
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Empty handler (may be for future use or debugging)
        }

        private void showimagegallery_Paint(object sender, PaintEventArgs e)
        {
            // Empty handler (may be for future use or debugging)
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Empty handler (may be for future use or debugging)
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Empty handler (may be for future use or debugging)
        }

        private void SettingBtn_Click_1(object sender, EventArgs e)
        {
            // Empty handler (may be for future use or debugging)
        }

        private void BtnBack_Click_1(object sender, EventArgs e)
        {

           
                if (folderHistory.Count > 1)
                {
                    folderHistory.Pop();
                    string parentFolder = folderHistory.Peek();
                    ShowContents(new string[] { parentFolder });
                }
                else
                {
                    MessageBox.Show("You are already at the root folder.");
                }
            }

        private async void button1_Click_1(object sender, EventArgs e)
        {

            FolderBrowserDialog folderDialog = new FolderBrowserDialog
            {
                Description = "Select a Folder",
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) // Default to Desktop
            };

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFolder = folderDialog.SelectedPath;
                imagespath.Text = selectedFolder;

                // 🔥 Save the selected folder to backend (.env)
                await logic.SaveDirectoryToBackend(selectedFolder);

                // 🔥 Get all image files from the selected folder and subfolders
                string[] imageFiles = Directory.GetFiles(selectedFolder, "*.*", SearchOption.AllDirectories)
                    .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                                   file.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
                    .ToArray();

                ShowImages(imageFiles);

                // 🔥 Send images to backend with relative paths
                await logic.ProcessImagesAsync(imageFiles, selectedFolder);
            }
        }

        private void pictureBox2_Click_1(object sender, EventArgs e)
        {
            this.Close(); // Close current form
            previousForm.Show();
        }
    }
    }
