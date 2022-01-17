using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;

namespace HrMan.Repository
{
    public class LeaveAllocationRepo : ILeaveAllocation
    {
        private readonly ApplicationDbContext _db;

        public LeaveAllocationRepo(ApplicationDbContext asd)
        {
            _db = asd;
        }

        

        public async Task<bool> Create(LeaveAllocation entity)
        {
            await _db.LeaveAllocations.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveAllocation entity)
        {
            _db.LeaveAllocations.Remove(entity);
            return  await Save();
        }

        public async Task<ICollection<LeaveAllocation>> FindAll()
        {
            return await _db.LeaveAllocations
                .Include(i=>i.Employee)
                .Include(i=>i.LeaveType)
                .ToListAsync();
        }

        public async Task<LeaveAllocation> FindById(int id)
        {
            var obj = await _db.LeaveAllocations
                .Include(i=> i.Employee)
                .Include(i=>i.LeaveType)
                .FirstOrDefaultAsync(i=>i.Id==id);
            return obj;
        }


        public async Task<bool> CheckAllocation(string empId, int leaveId)
        {
            var period = DateTime.Now.Year;
            var allocatin = await FindAll();
            return   allocatin.Where(i => i.EmployeeId == empId
                && i.LeaveTypeId == leaveId && i.Period == period).Any();
        }
       
      
        public async Task<bool> Save()
        {
            var obj =await _db.SaveChangesAsync();
            return obj > 0;
        }

        public async Task<bool> Update(LeaveAllocation entity)
        {
            _db.UpdateRange(entity);
            return await Save();
        }

        public async Task<IEnumerable<LeaveAllocation>> GetAllocationWithUser(string empId)
        {
            var allocatiion = await FindAll();
            return allocatiion.Where(u => u.EmployeeId == empId).ToList();
        }

        public async Task<LeaveAllocation> GetUserIdWithTypes(string empId, int leaveId)
        {
            var period = DateTime.Now.Year;
            var allocatiion = await FindAll();

            return allocatiion.FirstOrDefault(u => u.EmployeeId == empId && u.LeaveTypeId == leaveId && u.Period == period);

        }
 
   

        public async Task<bool> isExists(int id)
        {
            var obj = await _db.LeaveAllocations.AnyAsync(i => i.Id == id);
            return obj;
        }

        public async Task<LeaveAllocation> GetUserIdAllocation(string empId)
        {
            var allocatiion = await FindAll();

            return allocatiion.FirstOrDefault(u => u.EmployeeId == empId);

        }
    }
}
