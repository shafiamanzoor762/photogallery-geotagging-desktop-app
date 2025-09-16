using BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoGallery_Desktop
{
    public partial class Undo : Form
    {
        private Form previousForm;
        private List<UndoImage> undoList;


        public Undo(Form previousForm)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.Load += Undo_Load;
        }

        private async void Undo_Load(object sender, EventArgs e)
        {
            await GenerateImageCards();
        }
       


        private async Task GenerateImageCards()
        {
            //var resp = await Logic.get_undo_data() as List<UndoImage>;
            undoList = await Logic.get_undo_data() as List<UndoImage>;



            if (undoList == null || undoList.Count == 0)
            {
                label2.Visible = true;
                MessageBox.Show("No data or casting failed.");
                return;
            }

            int yOffset = 50;
            foreach (var image in undoList)
            {
                string path = image.path;
                Color baseColor = Color.CornflowerBlue;
                Panel card = new Panel
                {
                    Size = new Size(500, 150),
                    Location = new Point(20, yOffset),
                    BackColor = Color.FromArgb(175, baseColor.R, baseColor.G, baseColor.B),
                    BorderStyle = BorderStyle.FixedSingle
                };

                PictureBox picture = new PictureBox
                {
                    Size = new Size(100, 100),
                    Location = new Point(10, 25),
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                try
                {
                    picture.Image = System.Drawing.Image.FromFile(path);
                }
                catch
                {
                    picture.BackColor = Color.Gray;
                    picture.Image = null;
                }

                Button viewBtn = new Button
                {
                    Text = "View",
                    Location = new Point(350, 30),
                    Size = new Size(100, 30),
                    BackColor = Color.AliceBlue,
                    ForeColor = Color.MidnightBlue
                };
                viewBtn.Click += (s, e) =>
                {
                    int imageID = image.id;     // or get dynamically
                    int version = image.version_no;       // or get dynamically
                    this.Hide();
                    Details_Undo detailsForm = new Details_Undo(this, imageID, version);
                    detailsForm.Show();
                };

                Button undoBtn = new Button
                {
                    Text = "Undo",
                    Location = new Point(350, 70),
                    Size = new Size(100, 30),
                    BackColor = Color.MidnightBlue,
                    ForeColor = Color.AliceBlue
                };
                undoBtn.Click +=async (s, e) => {
                    int imageID = image.id;     // or get dynamically
                    int version = image.version_no;       // or get dynamically
                    bool result = await Logic.UndoData(imageID, version); // ✅ Await the async method

                    if (result)
                    {
                        MessageBox.Show("Undo successful!");
                    }
                    else
                    {
                        MessageBox.Show("Undo failed!");
                    }
                };

                card.Controls.Add(picture);
                card.Controls.Add(viewBtn);
                card.Controls.Add(undoBtn);
                this.Controls.Add(card);

                yOffset += 170;
            }

            this.AutoScroll = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close(); // Close current form
            previousForm.Show();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var data = await Logic.Bulk_undo(undoList);
            if (!string.IsNullOrEmpty(data.path))
            {
                MessageBox.Show(data.path);  
            }
            else
            {
                MessageBox.Show("Bulk undo failed.");
            }
        }
    }
}
