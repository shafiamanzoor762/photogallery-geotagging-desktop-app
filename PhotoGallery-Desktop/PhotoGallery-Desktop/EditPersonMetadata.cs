using BusinessLogicLayer;
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

		BindingList<Person> people = new BindingList<Person>();
		public EditPersonMetadata(BindingList<Person> people)
		{
			this.people = people;
			InitializeComponent();
		}

		private void EditPersonMetadata_Load(object sender, EventArgs e)
		{

			// Set background color
			this.BackColor = Color.PeachPuff;
			Panel mainPanel = new Panel
			{
				Size = new Size(470, 300),
				BackColor = Color.Teal,
				Location = new Point(10, 10),
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
					Size = new Size(200, 120),
					Location = new Point(x, y),
					BorderStyle = BorderStyle.None,
					BackColor = Color.White
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
					pictureBox.Image = Image.FromFile(person.Path);
				}
				catch
				{
					pictureBox.Image = Image.FromFile("C:\\Users\\lenovo\\Downloads\\PhotoGallery-Desktop\\PhotoGallery-Desktop\\images\\vectors\\button.png"); // Default image
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
					Text = person.Name,
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
					Checked = person.Gender == "F",
					Width = 60
				};

				RadioButton radioMale = new RadioButton
				{
					Text = "Male",
					Location = new Point(130, 80),
					Checked = person.Gender == "M",
					Width = 60
				};
				
				// Add controls to person panel
				personPanel.Controls.Add(pictureBox);
				personPanel.Controls.Add(nameLabel);
				personPanel.Controls.Add(nameTextBox);
				personPanel.Controls.Add(genderLabel);
				personPanel.Controls.Add(radioMale);
				personPanel.Controls.Add(radioFemale);

				// Add person panel to main panel
				mainPanel.Controls.Add(personPanel);

				// Adjust layout for next person
				count++;
				if (count % 2 == 0)
				{
					x = 20;
					y += 140;
				}
				else
				{
					x += 220;
				}
			}

			// Save Button
			Button saveButton = new Button
			{
				Text = "Save All",
				Size = new Size(100, 25),
				Location = new Point((mainPanel.Width - 100) / 2, y + 130),
				BackColor = Color.PaleVioletRed,
				ForeColor = Color.White,
			};
			saveButton.Click += SaveButton_Click;
			mainPanel.Controls.Add(saveButton);
		}

		// Save Button Click Event
		private void SaveButton_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Data saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

	}
}
