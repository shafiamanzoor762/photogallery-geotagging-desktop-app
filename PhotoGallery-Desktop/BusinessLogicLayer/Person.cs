using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
	public class Person
	{
        
            public int id { get; set; }
            public string name { get; set; }
            public string path { get; set; }  // Image path
            public string gender { get; set; } // "M" or "F"

            public Person(int id, string name, string path, string gender)
            {
                id = id;
                name = name;
                path = path;
                gender = gender;
            }
        
    }

}
