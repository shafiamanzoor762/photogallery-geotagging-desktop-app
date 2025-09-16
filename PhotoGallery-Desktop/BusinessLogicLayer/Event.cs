using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    internal class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        // Uncomment this if event_date exists in the database
        // [Required]
        // public DateTime EventDate { get; set; }

        // Many-to-Many Relationship with Image (through ImageEvent)
        public ICollection<Image> Images { get; set; } = new List<Image>();

        public override string ToString()
        {
            return $"<Event {Name}, Date: {EventDate}>";
        }
    }
}
