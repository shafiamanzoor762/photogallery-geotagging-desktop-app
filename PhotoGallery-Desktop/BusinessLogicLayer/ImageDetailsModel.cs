using System;

namespace BusinessLogicLayer
{
    public class ImageDetailsModel
    {

        public int id { get; set; }
        public string path { get; set; }
        public bool is_sync { get; set; }
        public DateTime capture_date { get; set; }
        public DateTime? event_date { get; set; }
        public DateTime last_modified { get; set; }
        public int? location_id { get; set; }
    }
}