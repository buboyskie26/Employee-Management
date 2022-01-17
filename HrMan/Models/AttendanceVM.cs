using HrMan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Models
{
    public class AttendanceVM
    {
        public int Id { get; set; }
        public int LoginCount { get; set; }
        public int OverTime { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LogOffDate { get; set; }
        public bool IsLogin { get; set; }
        public bool? IsLogOff { get; set; }
    
        public EmployeeVMTwo Employee { get; set; }
        public string EmployeeId { get; set; }
        public string Late { get; set; }
    }


    public class AttendanceEmployee
    {
        public List<EmployeeVMTwo> Employees { get; set; }
        public List<AttendanceVM> Attendances { get; set; }
        public int AttendanceId { get; set; }
        public int TotalHours { get; set; }
    }
    public class SingleAttendanceEmployee
    {
        public EmployeeVMTwo Employees { get; set; }
        public List<AttendanceVM> Attendances { get; set; }
    }
}
