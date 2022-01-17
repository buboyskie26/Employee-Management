using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Models
{
    public class EmployeeVM
    {
        public string Id { get; set; }
        public IFormFile ImageUrl { get; set; }
        public bool Login { get; set; }
        public int LoginCount { get; set; }
        public DateTime DateLogin { get; set; }

        public string UserName { get; set; }
        public int newUser { get; set; }
        public string Role { get; set; }

        public string contactNumber { get; set; }
        public string Email { get; set; }
        [Display(Name = "First Name")]

        public string Firstname { get; set; }
        [Display(Name ="Full Bame")]
        public string Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }


        [Display(Name = "First Bame")]

        public string Lastname { get; set; }
        [Display(Name = "Photo")]
        public string TaxId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
    }


    public class EmployeeVMTwo
    {
        public string Id { get; set; }
        public string ImageUrl { get; set; }
        public bool Login { get; set; }
        public int LoginCount { get; set; }
        public DateTime DateLogin { get; set; }
        public DateTime DateLogOff { get; set; }

        public string UserName { get; set; }
        public int newUser { get; set; }
        public string Role { get; set; }

        public string contactNumber { get; set; }
        public string Email { get; set; }
        [Display(Name = "First Name")]

        public string Firstname { get; set; }
        [Display(Name = "Full Bame")]
        public string Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }


        [Display(Name = "First Bame")]

        public string Lastname { get; set; }
        [Display(Name = "Photo")]
        public string TaxId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }
    }

    public class EmpRoleIndexVM
    {
        public List<EmployeeVMTwo> Employees { get; set; }
    }

   

    public class EmployeeIndexVM
    {
        public int newUser { get; set; }

        public string Id { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string contactNumber { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Fullname
        {
            get
            {
                return Firstname + " " + Lastname;
            }
        }
        public string Lastname { get; set; }
        [Display(Name = "Photo")]
        public string ImageUrl { get; set; }
        public string TaxId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateJoined { get; set; }

        public List<LeaveRequestVM> LeaveRequests { get; set; }
    }



}
