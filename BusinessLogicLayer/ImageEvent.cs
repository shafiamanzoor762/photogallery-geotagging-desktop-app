using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ImageEvent
    {
        public int ImageId { get; set; }  // Foreign Key to Image
        public int EventId { get; set; }  // Foreign Key to Event
    }
}
