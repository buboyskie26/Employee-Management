using HrMan.Contract;
using HrMan.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Repository
{
    public class AttendanceRepo : IAttendance
    {
        private readonly ApplicationDbContext _db;
        public AttendanceRepo(ApplicationDbContext asd)
        {
            _db = asd;
        }

      

        public async Task<bool> Create(Attendance entity)
        {
            await _db.Attendances.AddAsync(entity);
            return await Save();
        }

        public async Task<bool> Delete(Attendance entity)
        {
            _db.Attendances.Remove(entity);
            return await Save();
        }

        public async Task<ICollection<Attendance>> FindAll()
        {
            /*  var leaveTypes = await _db.Attendances.Where(i => i.UserName != "admin@gmail.com").ToListAsync();*/
            var leaveTypes = await _db.Attendances.Include(w=>w.Employee).ToListAsync();
            return leaveTypes;
        }

        public async Task<Attendance> FindById(int id)
        {
            var leaveType = await _db.Attendances.Include(u=>u.Employee)
                .FirstOrDefaultAsync(q=>q.Id==id);
            return leaveType;
        }

      /*  public async Task<IEnumerable<Attendance>> GetUserLogin(string user)
        {
            var obj = await FindAll();

            return obj.Where(i => i.Id == user).ToList();
        }*/

        public Task<bool> isExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Save()
        {
            var changes = await _db.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> Update(Attendance entity)
        {
            _db.Attendances.Update(entity);
            return await Save();
        }

        public async Task<bool> AlreadySignedIn(string userId)
        {
            var obj = await FindAll();

            return obj.Where(w => w.EmployeeId == userId && w.IsLogOff == null && w.IsLogin==true).Any();

        }

        public async Task<ICollection<Attendance>> GetEmployeeAttendace(string userId)
        {
            var obj = await FindAll();

            return obj.Where(i => i.EmployeeId == userId).ToList();
        }
        public async Task<bool> CheckEmployeeAttendace(bool first, bool? sec)
        {
            var obj = await FindAll();

            first = false;
            sec = null;


            return obj.Where(i => i.IsLogin == first && i.IsLogOff == sec).Any();
        }
        public async Task<Attendance> GetSingleEmployeeAttendace(int id, string userId)
        {

            var obj = await FindAll();

           /* var leaveType = await _db.Attendances
                 .FirstOrDefaultAsync(q => q.Id == id && q.EmployeeId == userId);*/
             

            return obj.Where(w=> w.Id == id && w.EmployeeId == userId).LastOrDefault();
        }
    }
}
