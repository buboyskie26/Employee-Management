using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;

namespace HrMan.Models
{
    public class LeaveAllocationVM
    {
        public int Id { get; set; }
        public int NumberOfDays { get; set; }
        public DateTime DateCreated { get; set; }
        public EmployeeVMTwo Employee { get; set; }
        public string EmployeeId { get; set; }
        public LeaveTypeVM LeaveType { get; set; }
        public int LeaveTypeId { get; set; }


        public IEnumerable<SelectListItem> Employees { get; set; }
        public IEnumerable<SelectListItem> LeaveTypes { get; set; }
        public int Period { get; set; }
    }
    public class DetailsAllocationVM
    {
        public EmployeeVMTwo Employees { get; set; }
        public List<LeaveAllocationVM> LeaveAllocations { get; set; }

    }
    public class SetLeaveVM
    {
        public string EmployeeId { get; set; }
        public int Period { get; set; }
        public DateTime DateCreated { get; set; }
        public int LeaveTypeId { get; set; }
        public int NumberOfDays { get; set; }

    }

    public class ListEmployeeVM
    {
     
        public List<Employee> Employees { get; set; }

    }
    public class AllocationIndex
    {
        public List<LeaveType> LeaveTypes { get; set; }
        public int userCount { get; set; }
    }
    public class EditLeaveAllocationVM
    {
        public int NumberOfDays { get; set; }

    }
}
