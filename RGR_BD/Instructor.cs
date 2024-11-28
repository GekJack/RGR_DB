using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_BD
{
    [Table("instructors")]
    public class Instructor
    {
        [Key]
        [Column("instructor_id")]
        public int InstructorId { get; set; }

        [Column("experiance_years")]
        public int ExperienceYears { get; set; }

        [Column("bio")]
        public string Bio { get; set; }

        [Column("rating")]
        public int Rating { get; set; }

        public ICollection<Session> Sessions { get; set; }
    }
}
