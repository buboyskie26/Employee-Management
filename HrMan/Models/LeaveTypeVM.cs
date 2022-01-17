using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Models
{
    public class LeaveTypeVM
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int DefaultDays { get; set; }

        public DateTime DateCreated { get; set; }
    }

}
