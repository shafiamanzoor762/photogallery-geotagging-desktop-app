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
	public partial class ShowPersonMetadata : Form
	{
		public static BindingList<Person> people;
		public ShowPersonMetadata(BindingList<Person> people)
		{
			ShowPersonMetadata.people = people;
			InitializeComponent();
		}

		private async void ShowPersonMetadata_Load(object sender, EventArgs e)
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
				Label nameTextLabel = new Label
				{
					Text = person.name,
					Location = new Point(70, 30),
					AutoSize = true
				};

				// Gender Label
				Label genderLabel = new Label
				{
					Text = "Gender",
					Location = new Point(70, 60),
					AutoSize = true
				};

				// Radio Buttons for Gender
				Label genderTextLabel = new Label
				{
					Text = person.gender == "F"? "Female" : "Male",
					Location = new Point(70, 80),
					//Checked = person.gender == "F",
					//Width = 60
					AutoSize = true
				};

				//RadioButton radioMale = new RadioButton
				//{
				//	Text = "Male",
				//	Location = new Point(130, 80),
				//	Checked = person.gender == "M",
				//	Width = 60
				//};

				// 🔄 **Bind UI to Data** 🔄
				//nameTextBox.TextChanged += (s, e1) => person.name = nameTextBox.Text;
				//names.Add(person.name);
				//radioFemale.CheckedChanged += (s, e1) => { if (radioFemale.Checked) person.gender = "F"; };
				//radioMale.CheckedChanged += (s, e1) => { if (radioMale.Checked) person.gender = "M"; };

				// Add controls to person panel
				personPanel.Controls.Add(pictureBox);
				personPanel.Controls.Add(nameLabel);
				personPanel.Controls.Add(nameTextLabel);
				personPanel.Controls.Add(genderLabel);
				//personPanel.Controls.Add(radioMale);
				personPanel.Controls.Add(genderTextLabel);

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
			}
	}
}
