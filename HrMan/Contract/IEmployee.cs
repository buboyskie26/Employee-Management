using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;

namespace HrMan.Contract
{
    public interface IEmployee : IRepositoryBase<Employee>
    {
        public Task<IEnumerable<Employee>> GetUserLogin(string user);
    }
}
