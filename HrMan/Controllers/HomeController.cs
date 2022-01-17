using AutoMapper;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HrMan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IAppointmentService _appointmentService;
        private readonly IEmployee _employee;
        private readonly ILeaveType _leaveType;
        private readonly ILeaveRequest _leaveRequest;
        private readonly ILeaveAllocation _leaveAllocation;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;

        public HomeController(ILeaveType leaveType,
                IMapper map, ILeaveAllocation leaveAllocation, IEmployee employee, ILeaveRequest leaveRequest,
                IAppointmentService appointmentService, ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor,
                UserManager<Employee> userManage)
        {
            _leaveRequest = leaveRequest;
            _leaveType = leaveType;
            _mapper = map;
            _leaveAllocation = leaveAllocation;
            _userManager = userManage;
            _appointmentService = appointmentService;
            _employee = employee;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }
      
        public IActionResult AccessDenied()
        {
            return View(0);
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View(new SampleViewModel());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> EmployeeSummary()
        {
            var request = await _leaveRequest.FindAll();
            var objRequest = _mapper.Map<List<LeaveRequestVM>>(request);

            var emp = _mapper.Map<List<EmployeeVMTwo>>(await _employee.FindAll());
            var obj = new RequestCountVM()
            {
                Employees = emp,
                LeaveRequests = objRequest
            };


            return View(obj);
        }
    }
}
