using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ImageRequest
    {
        public List<string> selectedEvents;

        public List<string> name { get; set; }
        public List<string> gender { get; set; }
        public List<string> events { get; set; }
        public List<string> capture_date { get; set; }

        public int age { get; set; }

        public Location location { get; set; }
    }
}
