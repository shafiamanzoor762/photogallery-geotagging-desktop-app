using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ImagePerson
    {
        public int ImageId { get; set; }  // Foreign Key to Image
        public int PersonId { get; set; }  // Foreign Key to Person
    }
}
