using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;
using HrMan.Models;

namespace HrMan.Contract
{
    public interface ILeaveRequest:IRepositoryBase<LeaveRequest>
    {
        Task<ICollection<LeaveRequest>> GetRequestByEmployeeId(string id);

        Task<bool> IsValidRequestCount(string userId );
        Task<ICollection<LeaveRequest>> CheckRequest(string userId);

        Task<DropDownVM> RequestDropdown(); 
    }
}
