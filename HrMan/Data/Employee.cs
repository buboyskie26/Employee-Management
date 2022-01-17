
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Data
{
    public class Employee : IdentityUser
    {
        public string Firstname { get; set; }
        public string Fullname { get; set; }
        public string Lastname { get; set; }
        public bool Login { get; set; }
        public int LoginCount { get; set; }

        public int newUser { get; set; }
        public string contactNumber { get; set; }
        public string TaxId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateLogOff { get; set; }
        public DateTime DateLogin { get; set; }
        public DateTime DateJoined { get; set; }
        public string ImageUrl { get; set; }
        public string Role { get; set; }
    }
}
