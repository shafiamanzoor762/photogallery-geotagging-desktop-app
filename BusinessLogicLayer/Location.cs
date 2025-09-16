using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business_Layer;

namespace BusinessLogicLayer
{
    public class Location
    {

        public int? Id { get; set; }  // Primary Key

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(10,8)")]
        public decimal? Latitude { get; set; }

        [Column(TypeName = "decimal(11,8)")]
        public decimal? Longitude { get; set; }

        public bool IsValid()
        {
            // Custom validation logic
            return !string.IsNullOrEmpty(Name);
        }
    }
}
