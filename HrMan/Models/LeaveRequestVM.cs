using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;

namespace HrMan.Models
{
    public class LeaveRequestVM
    {
        public int Id { get; set; }
        public int Count { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime DateRequested { get; set; }
        public string RequestComments { get; set; }
        public DateTime DateActioned { get; set; }
        public bool? Approved { get; set; }
        public bool Cancelled { get; set; }

        //
        public Employee ApprovedBy { get; set; }
        public string ApprovedById { get; set; }
        public Employee RequestingEmployee { get; set; }

        public string RequestingEmployeeId { get; set; }
 
        public LeaveType LeaveType { get; set; }

        public int LeaveTypeId { get; set; }

        public string Role { get; set; }

        //
        public string AdminId { get; set; }
        public string ManagerId { get; set; }
        public string RegularId { get; set; }
        public string SuperVisorId { get; set; }

        public string AdminName { get; set; }
        public string ManagerName { get; set; }
        public string RegularName { get; set; }
        public string SuperVisorName { get; set; }
    }

    public class RequestCountVM
    {
        public int TotalRequests { get; set; }
        public int ApproveRequests { get; set; }
        public int PendingRequests { get; set; }
        public int RejectRequests { get; set; }
        public List<LeaveRequestVM> LeaveRequests { get; set; }
        public List<EmployeeVMTwo> Employees { get; set; }
    }
    public class CreateRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; }= DateTime.UtcNow;
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        [Display(Name = "Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }

        [Display(Name = "Manager Name")]
        public string ManagerId { get; set; }
        [Display(Name = "Regular Name")]

        public string RegularId { get; set; }
        [Display(Name = "SuperVisor Name")]

        public string SuperVisorId { get; set; }

    }

    public class EditRequest
    {
        public int Id { get; set; }
        [Display(Name = "Start Date")]
        [Required]
        public DateTime StartDate { get; set; }  
        [Display(Name = "End Date")]
        [Required]
        public DateTime EndDate { get; set; } 
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        [Display(Name = "Leave Type")]
        public int LeaveTypeId { get; set; }
        [Display(Name = "Comments")]
        [MaxLength(300)]
        public string RequestComments { get; set; }

        [Display(Name = "Manager Name")]
        public string ManagerId { get; set; }
        [Display(Name = "Regular Name")]

        public string RegularId { get; set; }
        [Display(Name = "SuperVisor Name")]

        public string SuperVisorId { get; set; }

    }

    public class MyLeaveVM
    {
        public List<LeaveRequestVM> LeaveRequests { get; set; }

        public List<LeaveAllocationVM> LeaveAllocations { get; set; }
        /* public LeaveAllocationVM LeaveAllocations { get; set; }*/
    }

    

}
