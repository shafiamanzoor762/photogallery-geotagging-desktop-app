using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Layer
{
    public class Image
    {
        public int id { get; set; }
        public string path { get; set; }
        public int is_sync { get; set; }
        public DateTime capture_date { get; set; }
        public DateTime? event_date { get; set; }
        public DateTime last_modified { get; set; }
        public int? location_id { get; set; }

      





    }
}
