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

        }

        private void Mergefaces_Load(object sender, EventArgs e)
        {

        }
    }
}
