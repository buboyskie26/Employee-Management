using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;
using HrMan.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
 
namespace HrMan.Repository
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<Employee> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;


        public AppointmentService(ApplicationDbContext db,
            UserManager<Employee> userManager, IHttpContextAccessor httpContextAccessor
            )
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        [HttpPost]

        public async Task<int> AddUpdate(LeaveRequestVM m)
        {
            var startDate = Convert.ToDateTime(m.StartDate);
            var endDate = Convert.ToDateTime(m.EndDate);




            if (m != null && m.Id > 0)
            {
  
                return 1;
            }
            else
            {
 
                    LeaveRequest appointment = new LeaveRequest()
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Description = m.Description,
                        StartDate = Convert.ToDateTime( m.StartDate.ToString("dd/MM/yyyy h:mm tt")),
                        EndDate = Convert.ToDateTime(m.EndDate.ToString("dd/MM/yyyy h:mm tt")).AddDays(1),
                        LeaveTypeId = m.LeaveTypeId,
                        Approved = null,
                        Cancelled = true,
                        RequestComments = m.RequestComments,
                        DateRequested = DateTime.Now,
                        SuperVisorId = m.SuperVisorId,
                        ManagerId = m.ManagerId,
                        AdminId = m.AdminId,
                        RegularId = m.RegularId,
                        RequestingEmployeeId = loginUserId
                    };
                    await _db.LeaveRequests.AddAsync(appointment);
                    await _db.SaveChangesAsync();
               
 
                return 2;
            }
        }

        public LeaveRequestVM GetAllById(int id)
        {
            var obj = _db.LeaveRequests.Where(i => i.Id == id).Select(o => new LeaveRequestVM
            {
                Id = o.Id,
                StartDate = Convert.ToDateTime(o.StartDate.ToString("dd/MM/yyyy ")),
                EndDate = Convert.ToDateTime(o.EndDate.ToString("dd/MM/yyyy ")),
                Title=o.Title,
                Description=o.Description,
                RequestComments=o.RequestComments,
                LeaveTypeId=o.LeaveTypeId,
                SuperVisorId=o.SuperVisorId,
                ManagerId=o.ManagerId,
                RegularId=o.RegularId,
                Approved=o.Approved,
                SuperVisorName= _db.Users.Where(r=> r.Id == o.SuperVisorId).Select(r=> r.Firstname).FirstOrDefault(),
                RegularName= _db.Users.Where(r=> r.Id == o.RegularId).Select(r=> r.Firstname).FirstOrDefault(),
                ManagerName= _db.Users.Where(r=> r.Id == o.ManagerId).Select(r=> r.Firstname).FirstOrDefault(),
            }).FirstOrDefault();

            return obj;
        }

        public List<EmployeeVM> GetEmployeeList()
        {
            var obj =  (from user in _db.Employees
                        where user.UserName != "admin@gmail.com" && user.Firstname != "admin2"
                    select new EmployeeVM
                    {
                        Firstname = user.Fullname,
                        Id = user.Id
                    }).ToList();

            return obj;
        }

        public List<ManagerVM> GetManagerList()
        {
            var obj = (from user in _db.Users
                       join userRole in _db.UserRoles on user.Id equals userRole.UserId
                       join roles in _db.Roles.Where(u => u.Name == Helper.Manager) on userRole.RoleId equals roles.Id
                       select new ManagerVM
                       {
                           Name = user.Firstname,
                           Id = user.Id
                       }).ToList();


            return obj;
        }

        public List<RegularVM> GetRegularList()
        {
            var obj = (from user in _db.Users
                       join userRole in _db.UserRoles on user.Id equals userRole.UserId
                       join roles in _db.Roles.Where(u => u.Name == Helper.Regular) on userRole.RoleId equals roles.Id
                       select new RegularVM
                       {
                           Name = user.Firstname,
                           Id = user.Id
                       }).ToList();


            return obj;
        }

        public List<SuperVisorVM> GetSuperList()
        {
            var obj = (from user in _db.Users.Where(i=> i.Id == loginUserId)
                       join userRole in _db.UserRoles on user.Id equals userRole.UserId 
                       join roles in _db.Roles.Where(u => u.Name == Helper.SuperVisor) on userRole.RoleId equals roles.Id
                       select new SuperVisorVM
                       {
                           Name = user.Firstname,
                           Id = user.Id
                       }).ToList();


            return obj;
        }

        public List<LeaveRequestVM> ManagerEventsById(string managerId)
        {
            var obj = _db.LeaveRequests.Where(i => i.ManagerId == managerId).Select(o => new LeaveRequestVM
            {
                Title = o.Title,
                Description = o.Description,
                Approved = o.Approved,
                StartDate = Convert.ToDateTime(o.StartDate),
                EndDate = Convert.ToDateTime(o.EndDate),
                Id = o.Id,
                LeaveTypeId = o.LeaveTypeId,
            }).ToList();

            return obj;
        }

        public List<LeaveRequestVM> RegularEventsById(string regularId)
        {
            var obj = _db.LeaveRequests.Where(i => i.RegularId == regularId).Select(o => new LeaveRequestVM
            {
                Title = o.Title,
                Description=o.Description,
                Approved = o.Approved,
                StartDate = Convert.ToDateTime(o.StartDate.ToString("dd/MM/yyyy ")),
                EndDate = Convert.ToDateTime(o.EndDate.ToString("dd/MM/yyyy ")),
                Id = o.Id,
                LeaveTypeId = o.LeaveTypeId,
            }).ToList();

            return obj;
        }
        public List<LeaveRequestVM> SuperVisorEvents()
        {

            var objw = _db.LeaveRequests.ToList().Select(o => new LeaveRequestVM
            {
                StartDate = Convert.ToDateTime(o.StartDate),
                EndDate = Convert.ToDateTime(o.EndDate),
                Id = o.Id,
                LeaveTypeId = o.LeaveTypeId,
                Title = o.Title,
                Description = o.Description,
                Approved = o.Approved,
            }).ToList();
 
            return objw;
        }
        public List<LeaveRequestVM> SuperVisorEventsById(string superVisorId)
        {


            var obj = _db.LeaveRequests.Where(i => i.SuperVisorId == superVisorId).ToList().Select(o => new LeaveRequestVM
            {
                StartDate = Convert.ToDateTime(o.StartDate.ToString("dd/MM/yyyy h:mm tt")),
                EndDate = Convert.ToDateTime(o.EndDate.ToString("dd/MM/yyyy h:mm tt")),
                Id = o.Id,
                LeaveTypeId = o.LeaveTypeId,
                Title = o.Title,
                Description = o.Description,
                Approved = o.Approved,

            }).ToList();

            return obj;
        }
    }
}
