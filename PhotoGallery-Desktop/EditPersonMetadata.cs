using BusinessLogicLayer;
using DataAccessLayer2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoGallery_Desktop
{
    public partial class EditPersonMetadata : Form
    {

        public static BindingList<Person> people;
        public static BindingList<Person> peopledetails;
        public static BindingList<MergePeople> linkperson;
        //private EditImageModel imageDetails;
        private Mergefaces form = new Mergefaces();
        private FlowLayoutPanel panel;
        public EditPersonMetadata(BindingList<Person> people,BindingList<MergePeople> linkperson)
        {
            EditPersonMetadata.people = people;
            EditPersonMetadata.linkperson = linkperson;
            InitializeComponent();
        }

        //    private async void EditPersonMetadata_Load(object sender, EventArgs e)
        //    {
        //        //loadd all person data i.e name for merging


        //        // Fetch the data table asynchronously
        //        peopledetails = new BindingList<Person>(await Logic.GetAllPersons());
        //        //MessageBox.Show(peopledetails[0].name);
        //        // Check if the list is empty
        //        if (peopledetails == null || peopledetails.Count == 0)
        //        {
        //            MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }




        //        // Set background color
        //        this.BackColor = Color.White;
        //        Panel mainPanel = new Panel
        //        {
        //            Size = new Size(470, 300),
        //            BackColor = Color.RoyalBlue,
        //            Location = new Point(5, 5),
        //            BorderStyle = BorderStyle.None,
        //            AutoScroll = true
        //        };
        //        this.Controls.Add(mainPanel);

        //        int x = 20, y = 20;
        //        int count = 0;

        //        foreach (var person in people)
        //        {
        //            // Create a container Panel for each person
        //            Panel personPanel = new Panel
        //            {
        //                Size = new Size(200, 120),
        //                Location = new Point(x, y),
        //                BorderStyle = BorderStyle.None,
        //                BackColor = Color.White,
        //};

        //            // PictureBox for image
        //            PictureBox pictureBox = new PictureBox
        //            {
        //                Size = new Size(50, 50),
        //                Location = new Point(10, 10),
        //                SizeMode = PictureBoxSizeMode.StretchImage
        //            };
        //            try
        //            {
        //                pictureBox.Image = Image.FromStream(await Logic.LoadImageFromUrl(person.path));
        //            }
        //            catch
        //            {
        //                pictureBox.Image = Image.FromFile("C:\\Users\\shafia\\Downloads\\P\\PhotoGallery-Desktop\\PhotoGallery-Desktop\\images\\vectors\\date.png"); // Default image
        //            }

        //            // Name Label
        //            Label nameLabel = new Label
        //            {
        //                Text = "Name",
        //                Location = new Point(70, 10),
        //                AutoSize = true
        //            };

        //            // Name TextBox
        //            TextBox nameTextBox = new TextBox
        //            {
        //                Text = person.name,
        //                Location = new Point(70, 30),
        //                Width = 120
        //            };

        //            // Gender Label
        //            Label genderLabel = new Label
        //            {
        //                Text = "Gender",
        //                Location = new Point(70, 60),
        //                AutoSize = true
        //            };

        //            // Radio Buttons for Gender
        //            RadioButton radioFemale = new RadioButton
        //            {
        //                Text = "Female",
        //                Location = new Point(70, 80),
        //                Checked = person.gender == "F",
        //                Width = 60
        //            };

        //            RadioButton radioMale = new RadioButton
        //            {
        //                Text = "Male",
        //                Location = new Point(130, 80),
        //                Checked = person.gender == "M",
        //                Width = 60
        //            };

        //            // 🔄 **Bind UI to Data** 🔄
        //            nameTextBox.TextChanged += (s, e1) => person.name = nameTextBox.Text;
        //            //names.Add(person.name);
        //            radioFemale.CheckedChanged += (s, e1) => { if (radioFemale.Checked) person.gender = "F"; };
        //            radioMale.CheckedChanged += (s, e1) => { if (radioMale.Checked) person.gender = "M"; };

        //            // Add controls to person panel
        //            personPanel.Controls.Add(pictureBox);
        //            personPanel.Controls.Add(nameLabel);
        //            personPanel.Controls.Add(nameTextBox);
        //            personPanel.Controls.Add(genderLabel);
        //            personPanel.Controls.Add(radioMale);
        //            personPanel.Controls.Add(radioFemale);

        //            // Add person panel to main panel
        //            mainPanel.Controls.Add(personPanel);

        //            // Adjust layout for next person
        //            count++;
        //            if (count % 2 == 0)
        //            {
        //                x = 20;
        //                y += 140;
        //            }
        //            else
        //            {
        //                x += 220;
        //            }
        //        }

        //        // Save Button
        //        Button saveButton = new Button
        //        {
        //            Text = "Save All",
        //            Size = new Size(100, 25),
        //            Location = new Point((mainPanel.Width - 100) / 2, y + 130),
        //            BackColor = Color.PaleVioletRed,
        //            ForeColor = Color.White,
        //        };
        //        saveButton.Click += SaveButton_Click;
        //        mainPanel.Controls.Add(saveButton);




        //    }

        private async void EditPersonMetadata_Load(object sender, EventArgs e)
        {
            //loadd all person data i.e name for merging


            // Fetch the data table asynchronously
            peopledetails = new BindingList<Person>(await Logic.GetAllPersons());
            //MessageBox.Show(peopledetails[0].name);
            // Check if the list is empty
            if (peopledetails == null || peopledetails.Count == 0)
            {
                MessageBox.Show("No data received from the API.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }




            // Set background color
            this.BackColor = Color.White;
            Panel mainPanel = new Panel
            {
                Size = new Size(470, 300),
                BackColor = Color.MidnightBlue,
                Location = new Point(5, 5),
                BorderStyle = BorderStyle.None,
                AutoScroll = true
            };
            this.Controls.Add(mainPanel);

            int x = 20, y = 20;
            int count = 0;

            foreach (var person in people)
            {
                // Create a container Panel for each person
                Panel personPanel = new Panel
                {
                    Size = new Size(200, 170),
                    Location = new Point(x, y),
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.White,
                };

                // PictureBox for image
                PictureBox pictureBox = new PictureBox
                {
                    Size = new Size(50, 50),
                    Location = new Point(10, 10),
                    SizeMode = PictureBoxSizeMode.StretchImage
                };
                try
                {
                    pictureBox.Image = Image.FromStream(await Logic.LoadImageFromUrl(person.path));
                }
                catch
                {
                    pictureBox.Image = Image.FromFile("C:\\Users\\shafia\\Downloads\\P\\PhotoGallery-Desktop\\PhotoGallery-Desktop\\images\\vectors\\date.png"); // Default image
                }

                // Name Label
                Label nameLabel = new Label
                {
                    Text = "Name",
                    Location = new Point(70, 10),
                    AutoSize = true
                };

                // Name TextBox
                TextBox nameTextBox = new TextBox
                {
                    Text = person.name,
                    Location = new Point(70, 30),
                    Width = 120
                };

                // Gender Label
                Label genderLabel = new Label
                {
                    Text = "Gender",
                    Location = new Point(70, 60),
                    AutoSize = true
                };

                // Radio Buttons for Gender
                RadioButton radioFemale = new RadioButton
                {
                    Text = "Female",
                    Location = new Point(70, 80),
                    Checked = person.gender == "F",
                    Width = 60
                };

                RadioButton radioMale = new RadioButton
                {
                    Text = "Male",
                    Location = new Point(130, 80),
                    Checked = person.gender == "M",
                    Width = 60
                };

                // DOB Label (below gender)
                Label dobLabel = new Label
                {
                    Text = "DOB",
                    Location = new Point(10, 120),  // shifted below gender
                    AutoSize = true
                };

                DateTimePicker dobPicker = new DateTimePicker
                {
                    Format = DateTimePickerFormat.Short,
                    Location = new Point(60, 120),
                    Width = 120,
                    Value = person.dob ?? DateTime.Today // ✅ Use null-coalescing operator
                };

                // ✅ Set it immediately to sync with UI
                person.dob = dobPicker.Value;

                // ✅ Also keep this to allow dynamic updates
                dobPicker.ValueChanged += (s, e1) => person.dob = dobPicker.Value;



                // 🔄 **Bind UI to Data** 🔄
                nameTextBox.TextChanged += (s, e1) => person.name = nameTextBox.Text;
                //names.Add(person.name);
                radioFemale.CheckedChanged += (s, e1) => { if (radioFemale.Checked) person.gender = "F"; };
                radioMale.CheckedChanged += (s, e1) => { if (radioMale.Checked) person.gender = "M"; };

                // Add controls to person panel
                personPanel.Controls.Add(pictureBox);
                personPanel.Controls.Add(nameLabel);
                personPanel.Controls.Add(nameTextBox);
                personPanel.Controls.Add(genderLabel);
                personPanel.Controls.Add(radioMale);
                personPanel.Controls.Add(radioFemale);
                personPanel.Controls.Add(dobLabel);
                personPanel.Controls.Add(dobPicker);


                // Add person panel to main panel
                mainPanel.Controls.Add(personPanel);


                // Adjust layout for next person
                count++;
                //if (count % 2 == 0)
                //{
                //    x = 20;
                //    y += 190;
                //}
                //else
                //{
                //    x += 220;
                //}
                if (count % 2 == 0)
                {
                    x = 20;
                    y += personPanel.Height + 20;  // 🔄 Dynamic spacing
                }
                else
                {
                    x += personPanel.Width + 20;   // 🔄 Dynamic spacing
                }

            }

            // Save Button
            // Determine final button Y position dynamically
            int buttonY = (count % 2 == 0) ? y : y + 170 + 20; // 170 = personPanel.Height, 20 = spacing

            // Save Button
            Button saveButton = new Button
            {
                Text = "Save All",
                Size = new Size(100, 30),
                Location = new Point((mainPanel.Width - 100) / 2, buttonY),
                BackColor = Color.AliceBlue,
                ForeColor = Color.MidnightBlue,
            };
            saveButton.Click += SaveButton_Click;
            mainPanel.Controls.Add(saveButton);


        }
        /// <summary>
        /// ///for different this function
        
        /// </summary>
        public List<(int Id, string ImageName)> sameClickedData = new List<(int, string)>();

        private void HandleSameClicked(int id, string imageName)
        {
            linkperson.Add(new MergePeople
            {
                person1_id = id,
                person2_emb_name = imageName
            });

            MessageBox.Show($"Same clicked for ID: {id}, Image: {imageName}");



        }

        private void DisplayGroups(Person person, Dictionary<string, List<string>> groups, FlowLayoutPanel parentPanel, TextBox nameTextBox)
        {
            foreach (var entry in groups)
            {
                string mainImage = entry.Key;
                List<string> others = entry.Value;

                Panel groupPanel = new Panel
                {
                    Width = 440,
                    Height = 230,
                    Margin = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // 1. Label and Person Image
                Label titleLabel = new Label
                {
                    Text = "Link person with this group:",
                    Location = new Point(10, 10),
                    AutoSize = true,
                    Font = new Font("Arial", 9, FontStyle.Bold)
                };
                groupPanel.Controls.Add(titleLabel);

                PictureBox personImage = new PictureBox
                {
                    Width = 60,
                    Height = 60,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(titleLabel.Right + 10, 5)
                };
                try
                {
                    personImage.Load(person.path);
                }
                catch
                {
                    // Handle error
                }
                groupPanel.Controls.Add(personImage);

                // 2. Horizontal scrollable image panel
                Panel scrollImagePanel = new Panel
                {
                    Location = new Point(10, 75),
                    Size = new Size(410, 70),
                    AutoScroll = true,
                    BorderStyle = BorderStyle.None

                };

                int imgX = 0;
                //var allImages = new List<string> { mainImage };
                var allImages = new List<string>(others);

                //allImages.AddRange(others);

                foreach (var imagePath in allImages)
                {
                    PictureBox pic = new PictureBox
                    {
                        Width = 60,
                        Height = 60,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Location = new Point(imgX, 0),
                        Margin = new Padding(0),
                    };

                    try
                    {
                        pic.Load($"{Api.BaseUrl}/face_images/{imagePath}");
                    }
                    catch
                    {
                        // Optional: Placeholder
                    }

                    scrollImagePanel.Controls.Add(pic);
                    imgX += 70;
                }

                groupPanel.Controls.Add(scrollImagePanel);

                // 3. Buttons
                Button btnSame = new Button
                {
                    Text = "Same",
                    Width = 100,
                    Location = new Point(10, 160)
                };
                //btnSame.Click += (s, e) => MessageBox.Show($"Same clicked for {mainImage}");
                btnSame.Click += (s, e) => HandleSameClicked(person.id, mainImage);


                Button btnDiff = new Button
                {
                    Text = "Different",
                    Width = 100,
                    Location = new Point(120, 160)
                };
                btnDiff.Click += async (s, e) =>
                {
                    MessageBox.Show("If this person is different, please give a unique name.");
                    await Task.Delay(2000);
                    nameTextBox.Focus();
                    nameTextBox.SelectAll(); // Optional: select all text
                };


                groupPanel.Controls.Add(btnSame);
                groupPanel.Controls.Add(btnDiff);

                parentPanel.Controls.Add(groupPanel);
            }

        }



        private TextBox FindNameTextBoxForPerson(Person person)
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel mainPanel)
                {
                    foreach (Control subPanel in mainPanel.Controls)
                    {
                        if (subPanel is Panel personPanel)
                        {
                            foreach (Control ctrl in personPanel.Controls)
                            {
                                if (ctrl is TextBox tb && tb.Text == person.name)
                                {
                                    return tb;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }


        // Save Button Click Event
        private async void SaveButton_Click(object sender, EventArgs e)
        {
            form.Size = new Size(500, 400); // Increase form size to fit buttons


            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                //Dock = DockStyle.Fill // Or manually set size/position
                Location = new Point(20, 60), // margin from top-left
                Size = new Size(460, 300)     // adjust to leave space for buttons

            };
            form.Controls.Add(flowPanel);



            foreach (var person in people)  //cropped face
            {
                foreach (var personDetails in peopledetails)  //db person
                {
                    if (string.Equals(person.name, "unknown", StringComparison.OrdinalIgnoreCase) ||
                       string.Equals(personDetails.name, "unknown", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    //if (string.Equals(person.name, personDetails.name, StringComparison.OrdinalIgnoreCase))
                    if (string.Equals(person.name?.Trim(), personDetails.name?.Trim(), StringComparison.OrdinalIgnoreCase))

                    {
                        if (person.path != personDetails.path) // Check if paths are different
                        {

                            try
                            {
                                var resp = await Logic.SendPersonToBackend(person, person.name);
                                var groups = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(resp);

                                if (groups != null && groups.Count > 0)
                                {
                                    form.Show();
                                    // Find nameTextBox again from mainPanel:
                                    TextBox nameTextBox = FindNameTextBoxForPerson(person);
                                    DisplayGroups(person, groups, flowPanel, nameTextBox);

                                }
                            }

                            catch
                            {

                            }
                            break;
                        }
                    }


                }

            }

        }
       

       
    }
}





