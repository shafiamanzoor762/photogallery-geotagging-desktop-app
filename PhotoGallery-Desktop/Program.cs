using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoGallery_Desktop
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new EditImage());
            //Application.Run(new SearchImage());
            Application.Run(new FirstScreen());
            //Application.Run(new EDDB());
            //Application.Run(new Taskcs());
            //Application.Run(new GetPersonsGroupFromImage());
        }
    }
}
