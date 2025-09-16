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
    public partial class Mergefaces : Form
    {
        public PictureBox PictureBox { get; set; }
        public CheckBox CheckBox { get; set; }

        public Mergefaces()
        {
            InitializeComponent();


            PictureBox = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(100, 100)  // Size of the picture
            };

            CheckBox = new CheckBox
            {
                Text = "Merge Faces",
                Checked = false // Initially unchecked
            };

            this.Controls.Add(PictureBox);
            this.Controls.Add(CheckBox);
        }

        private void Mergefaces_Load(object sender, EventArgs e)
        {

        }
    }
}
