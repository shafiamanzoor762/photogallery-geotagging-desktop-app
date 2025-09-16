using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
	public class EditImageModel
	{
		public int id { get; set; }
		public string path { get; set; }
		public Boolean is_sync { get; set; }
		public DateTime capture_date { get; set; }
		public DateTime? event_date { get; set; }
		public DateTime? last_modified { get; set; }
		public Location location { get; set; }
		public Person[] persons { get; set; }
		public Event[] events { get; set; }
	}
}
