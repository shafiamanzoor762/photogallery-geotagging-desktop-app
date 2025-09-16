using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class GroupedImageResult
    {
        public Dictionary<string, List<string>> events { get; set; }
        public Dictionary<string, List<string>> locations { get; set; }
        public Dictionary<string, List<string>> capture_dates { get; set; }
        public Dictionary<string, List<string>> event_dates { get; set; }
    }
}
