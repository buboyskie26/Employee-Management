using AutoMapper;
using HrMan.Contract;
using HrMan.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HrMan.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IAppointmentService _appointmentService;

        private readonly ILeaveType _leaveType;
        private readonly ILeaveAllocation _leaveAllocation;
        private readonly UserManager<Employee> _userManager;
        private readonly IEmployee _employee;
        private readonly ILeaveRequest _leaveRequest;
        private readonly IMapper _mapper;

        private readonly string role;
        private readonly string loginUserId;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static Random _rnd = new Random();

        public EmployeeController(ILeaveType leaveType, IEmployee employee,
                IMapper map, ILeaveAllocation leaveAllocation,
                IAppointmentService appointmentService,
                UserManager<Employee> userManage, ILeaveRequest leaveRequest,
                IHttpContextAccessor httpCotext)
        {

            _httpContextAccessor = httpCotext;
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _leaveType = leaveType;
            _mapper = map;
            _leaveAllocation = leaveAllocation;
            _userManager = userManage;
            _leaveRequest = leaveRequest;
            _appointmentService = appointmentService;
            _employee = employee;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new SampleViewModel());
        }


        [HttpPost]
        public JsonResult GetAnswer(string question)
        {
            int index = _rnd.Next(_db.Count);
            var answer = _db[index];
            return Json(answer);
        }


        private static List<string> _db = new List<string> { "Yes", "No", "Definitely, yes", "I don't know", "Looks like, yes" };
    }
}
