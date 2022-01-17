using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Data
{
    public class Attendance
    {
        [Key]
        public int Id { get; set; }
        public int LoginCount { get; set; }
        public int OverTime { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LogOffDate { get; set; }
        public bool IsLogin { get; set; }
        public bool? IsLogOff { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public string EmployeeId { get; set; }
        public string Late { get; set; }


    }
}
