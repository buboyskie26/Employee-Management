using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;

namespace HrMan.Contract
{
    public interface IAttendance : IRepositoryBase<Attendance>
    {
        public Task<bool> AlreadySignedIn(string userId);
        public Task<ICollection<Attendance>> GetEmployeeAttendace(string userId);
        public Task<bool> CheckEmployeeAttendace(bool fi,bool? sec);
        public Task<Attendance> GetSingleEmployeeAttendace(int id, string userId);
    }
}
