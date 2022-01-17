using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;

namespace HrMan.Repository
{
    public class EmployeeRepo : IEmployee
    {
        private readonly ApplicationDbContext _db;
        public EmployeeRepo(ApplicationDbContext asd)
        {
            _db = asd;
        }

        public async Task<bool> Create(Employee entity)
        {
            await _db.Employees.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Employee entity)
        {
            _db.Employees.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<Employee>> FindAll()
        {
          /*  var leaveTypes = await _db.Employees.Where(i => i.UserName != "admin@gmail.com").ToListAsync();*/
            var leaveTypes = await _db.Employees.ToListAsync();
            return leaveTypes;
        }

        public async Task<Employee> FindById(int id)
        {
            var leaveType = await _db.Employees.FindAsync(id);
            return leaveType;
        }

        public async Task<IEnumerable<Employee>> GetUserLogin(string user)
        {
            var obj = await FindAll();

            return obj.Where(i => i.Id == user).ToList();
        }

        public Task<bool> isExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Employee entity)
        {
            _db.Employees.Update(entity);
            return await Save();
        }
    }
}
