using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{

    /// <summary>
    /// Newwwwwwwwwwwwwwwwwww
    /// </summary>
	public class Person
	{
        
            public int id { get; set; }
            public string name { get; set; }
            public string path { get; set; }  // Image path
            public string gender { get; set; } // "M" or "F"

            public DateTime? dob { get; set; } // ← Make sure this is added

            public int? age { get; set; }

    }

}
