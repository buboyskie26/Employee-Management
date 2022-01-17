using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;
using HrMan.Contract;

namespace HrMan.Contract
{
    public interface ILeaveAllocation : IRepositoryBase<LeaveAllocation>
    {
        public  Task<bool> CheckAllocation(string empId, int leaveId);

        public Task<IEnumerable<LeaveAllocation>> GetAllocationWithUser(string empId);

        public Task<LeaveAllocation> GetUserIdWithTypes(string empId, int leaveId);
        public Task<LeaveAllocation> GetUserIdAllocation(string empId);
    }
}
