﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGR_BD
{
    [Table("client")]
    public class Client
    {
        [Key]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Column("date_of_registration")]
        public DateTime DateOfRegistration { get; set; }

        public ICollection<Bookings> Bookings { get; set; }
    }
}
