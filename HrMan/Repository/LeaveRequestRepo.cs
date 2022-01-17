using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;

namespace HrMan.Repository
{
    public class LeaveRequestRepo : ILeaveRequest
    {
        private readonly ApplicationDbContext _db;
        public LeaveRequestRepo(ApplicationDbContext asd)
        {
            _db = asd;
        }

        public async Task<bool> Create(LeaveRequest entity)
        {
            await _db.LeaveRequests.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(LeaveRequest entity)
        {
            _db.LeaveRequests.RemoveRange(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> FindAll()
        {
            var leave = await _db.LeaveRequests
                .Include(i => i.RequestingEmployee)
                .Include(i => i.ApprovedBy)
                .Include(i => i.LeaveType)
                .ToListAsync();
            return leave;
        }

     

        public async Task<ICollection<LeaveRequest>> GetEmployeesByLeaveType(int id)
        {
            var request = await FindAll();

            return request.Where(r => r.LeaveTypeId == id).ToList();
        }

        public async Task<bool> isExists(int id)
        {
            var exists = await _db.LeaveRequests.AnyAsync(q => q.Id == id);
            return exists;
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(LeaveRequest entity)
        {
            _db.LeaveRequests.Update(entity);
            return await Save();
        }

        public async Task<ICollection<LeaveRequest>> GetRequestByEmployeeId(string id)
        {
            var request = await FindAll();
            return request.Where(i => i.RequestingEmployeeId == id).ToList();

        }

      
        public async Task<DropDownVM> RequestDropdown()
        {
            var drop = new DropDownVM()
            {
                LeaveTypes = await _db.LeaveTypes.OrderBy( i=> i.Name).ToListAsync()
            };
            return drop;
        }

        public async Task<LeaveRequest> FindById(int id)
        {
            var leaveType = await _db.LeaveRequests
               .Include(i => i.RequestingEmployee)
               .Include(i => i.ApprovedBy)
               .Include(i => i.LeaveType)
               .FirstOrDefaultAsync(i => i.Id == id);
            return leaveType;
        }
        /*public async Task<bool> CheckAllocation(string empId, int leaveId)
        {
            var period = DateTime.Now.Year;
            var allocatin = await FindAll();
            return allocatin.Where(i => i.EmployeeId == empId
              && i.LeaveTypeId == leaveId && i.Period == period).Any();
        }*/
        public async Task<bool> IsValidRequestCount(string userId )
        {

            var request = await FindAll();

           /* int count = request.Count(i => i.Approved == null);*/
            return request.Where(i=> i.RequestingEmployeeId == userId).Any();
        }

        public async Task<ICollection<LeaveRequest>> CheckRequest(string userId)
        {
            var obj = await FindAll();

            int count = obj.Count(o => o.Approved == null);

            return obj.Where(i => i.RequestingEmployeeId == userId).ToList();
        }
    }
}
