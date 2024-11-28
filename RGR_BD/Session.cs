using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_BD
{
    [Table("session")]
    public class Session
    {
        [Key]
        [Column("session_id")]
        public int SessionId { get; set; }

        [Column("instructor_id")]
        public int InstructorId { get; set; }

        [Column("location_id")]
        public int LocationId { get; set; }

        [Column("start_time")]
        public DateTime StartTime { get; set; }

        [Column("end_time")]
        public DateTime EndTime { get; set; }

        [Column("max_participants")]
        public int MaxParticipants { get; set; }

        [Column("price", TypeName = "money")]
        public decimal PriceMoney { get; set; }

        public Instructor Instructor { get; set; }
        public Location Location { get; set; }
        public ICollection<Bookings> Bookings { get; set; }
    }
}
