using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;

namespace HrMan.Models
{
    public class DropDownVM
    {
        public DropDownVM()
        {
            Employees = new List<Employee>();
            TaxYears = new List<TaxYear>();
            LeaveRequests = new List<LeaveRequest>();
            LeaveTypes = new List<LeaveType>();
        }
        
        public List<Employee> Employees { get; set; }
        public List<TaxYear> TaxYears { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; }
        public List<LeaveType> LeaveTypes { get; set; }
    }
}
