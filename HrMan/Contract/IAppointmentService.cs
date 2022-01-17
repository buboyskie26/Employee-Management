using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Models;

namespace HrMan.Contract
{
    public interface IAppointmentService
    {
        public Task<int> AddUpdate(LeaveRequestVM model);

        List<EmployeeVM> GetEmployeeList();
        List<ManagerVM> GetManagerList();
        List<RegularVM> GetRegularList();
        List<SuperVisorVM> GetSuperList();
        List<LeaveRequestVM> SuperVisorEvents();
        List<LeaveRequestVM> SuperVisorEventsById(string superId);
        List<LeaveRequestVM> ManagerEventsById(string superId);
        List<LeaveRequestVM> RegularEventsById(string superId);

        LeaveRequestVM GetAllById(int id);
    }
}
