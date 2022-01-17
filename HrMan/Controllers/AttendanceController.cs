using AutoMapper;
using HrMan.Contract;
using HrMan.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Models;

namespace HrMan.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAppointmentService _appointmentService;

        private readonly IAttendance _attendance;
        private readonly ILeaveAllocation _leaveAllocation;
        private readonly UserManager<Employee> _userManager;
        private readonly IEmployee _employee;
        private readonly ILeaveRequest _leaveRequest;
        private readonly IMapper _mapper;

        private readonly string role;
        private readonly string loginUserId;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AttendanceController(IAttendance att, IEmployee employee,
                IMapper map, ILeaveAllocation leaveAllocation,
                IAppointmentService appointmentService,
                UserManager<Employee> userManage, ILeaveRequest leaveRequest,
                IHttpContextAccessor httpCotext)
        {
            _httpContextAccessor = httpCotext;

            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _attendance = att;
            _mapper = map;
            _leaveAllocation = leaveAllocation;
            _userManager = userManage;
            _leaveRequest = leaveRequest;
            _appointmentService = appointmentService;
            _employee = employee;
        }

        [Authorize]

        public async Task<IActionResult> Index()
        {
 
            var user = await _employee.FindAll();

            var emp = _mapper.Map<List<EmployeeVMTwo>>(user);

            var attend = await _attendance.FindAll();
            var attendMap = _mapper.Map<List<AttendanceVM>>(attend);

            var obj = new AttendanceEmployee()
            {
                Employees = emp,
                Attendances = attendMap
            };

            return View(obj);
        }

        public async Task<IActionResult> SignIn()
        {
            var user = _userManager.GetUserAsync(User).Result;

            var obj = await _employee.GetUserLogin(user.Id);

            var emp = _mapper.Map<List<EmployeeVMTwo>>(obj);

            var attend = await _attendance.FindAll();

            var attendObj = _mapper.Map<List<AttendanceVM>>(attend);

            var attendace = new AttendanceEmployee()
            {
                Employees = emp,
                Attendances = attendObj,
                AttendanceId = attend.Select(i => i.Id).FirstOrDefault()
            };

            return View(attendace);
        }
        public async Task<IActionResult> TimeIn(string id)
        {
            try
            {

                var user = _userManager.FindByIdAsync(id).Result;

                var attend = await _attendance.GetEmployeeAttendace(user.Id);

                var attendObj = attend.Select(i => i.LoginDate).FirstOrDefault();


                if(await _attendance.AlreadySignedIn(user.Id))
                {
                    ModelState.AddModelError("", "Time out first");
                    return RedirectToAction("SignIn");
                }
                    

        /*        foreach (var item in attend)
                {
                     
                    if (item.IsLogin.Equals(true) && item.IsLogOff == null && user.Id.Equals(item.EmployeeId))
                        continue;
                }*/


                var obj = new AttendanceVM()
                {
                    EmployeeId = user.Id,
                    LoginDate = DateTime.Now,
                    IsLogin=true,
                    IsLogOff=null
                };
                obj.LoginCount += 1;

                var startDate = new DateTime((int)DateTime.UtcNow.Year, (int)DateTime.UtcNow.Month, (int)DateTime.UtcNow.Day, 23, 0, 0, DateTimeKind.Utc);


                if(obj.LoginDate > startDate)
                {
                    ModelState.AddModelError("", "Pass 5 PM");
                    return RedirectToAction("SignIn");
                }
                 
                var objList = _mapper.Map<Attendance>(obj);

                var success = await _attendance.Create(objList);


                if(!success)
                {
                    ModelState.AddModelError("", "Not Success");
                    return RedirectToAction("SignIn");
                }

                return RedirectToAction("SignIn");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Invalid");
                return RedirectToAction("SignIn");

            }

        }

        public async Task<IActionResult> TimeOut(int id)
        {

            var user = _userManager.GetUserAsync(User).Result;

            var obj = await _attendance.GetSingleEmployeeAttendace(id,user.Id);
            
            obj.LogOffDate = DateTime.Now;
            obj.IsLogin = false;
            obj.IsLogOff = true;
            var success = await _attendance.Update(obj);
              if (!success)
                      {
                          ModelState.AddModelError("", "Not Success");
                          return RedirectToAction("SignIn");
                      }
            return RedirectToAction("SignIn");
 
        }

        public async Task<IActionResult> AttendanceEmployee(string id)
        {

            var user = _userManager.FindByIdAsync(id).Result;

            var empMap = _mapper.Map<EmployeeVMTwo>(user);

            var obj = await _attendance.GetEmployeeAttendace(id);

            var map = _mapper.Map<List<AttendanceVM>>(obj);

            var start = obj.Select(i => i.LoginDate);
            var end = obj.Select(i => i.LogOffDate);

            
            var objMap = new SingleAttendanceEmployee()
            {
                Attendances = map,
                Employees = empMap
            };
            return View(objMap);

        }
    }
}
