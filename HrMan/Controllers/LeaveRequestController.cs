using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace HrMan.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
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

        public LeaveRequestController(ILeaveType leaveType, IEmployee employee,
                IMapper map, ILeaveAllocation leaveAllocation,
                IAppointmentService appointmentService,
                UserManager<Employee> userManage, ILeaveRequest leaveRequest,
                IHttpContextAccessor  httpCotext)
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

        /*
                [Authorize(Roles ="Admin,Administrator,SuperVisor")]*/
        public async Task<IActionResult> Index()
        {
            var request = await _leaveRequest.FindAll();

            var dropdo = await _leaveRequest.RequestDropdown();
            ViewBag.leaveName = new SelectList(dropdo.LeaveTypes, "Id", "Name");

            ViewBag.Employee = _appointmentService.GetEmployeeList();
            ViewBag.Supervisor = _appointmentService.GetSuperList();
            ViewBag.Manager = _appointmentService.GetManagerList();
            ViewBag.Regular = _appointmentService.GetRegularList();


            var objRequest = _mapper.Map<List<LeaveRequestVM>>(request);

            var obj = new RequestCountVM()
            {
                TotalRequests = request.Count,
                RejectRequests = request.Count(u => u.Approved == false),
                ApproveRequests = request.Count(u => u.Approved == true),
                PendingRequests = request.Count(u => u.Approved == null),
                LeaveRequests = objRequest
            };
            return View(obj);
        }

        public async Task<IActionResult> MyLeave()
        {
            /* var emp = await _employee.FindAll();*/

            var user = _userManager.GetUserAsync(User).Result;
            var request = _mapper.Map<List<LeaveRequestVM>>(await _leaveRequest.GetRequestByEmployeeId(user.Id));
            var allocation = _mapper.Map<List<LeaveAllocationVM>>(await _leaveAllocation.GetAllocationWithUser(user.Id));

            var dropdo = await _leaveRequest.RequestDropdown();
            ViewBag.leaveName = new SelectList(dropdo.LeaveTypes, "Id", "Name");

            ViewBag.Employee = _appointmentService.GetEmployeeList();
            ViewBag.Supervisor = _appointmentService.GetSuperList();
            ViewBag.Manager = _appointmentService.GetManagerList();
            ViewBag.Regular = _appointmentService.GetRegularList();

            /*var allocation = _mapper.Map<LeaveAllocationVM>(await _leaveAllocation.GetUserIdAllocation(user.Id));*/
            var obj = new MyLeaveVM()
            {
                LeaveAllocations = allocation,
                LeaveRequests = request
            };
            return View(obj);
        }
      
        public async Task<IActionResult> Create()
        {
            var leave = await _leaveType.FindAll();

            var dropdo = await _leaveRequest.RequestDropdown();
            ViewBag.leaveName = new SelectList(dropdo.LeaveTypes, "Id", "Name");

            ViewBag.Employee = _appointmentService.GetEmployeeList();
            ViewBag.Supervisor = _appointmentService.GetSuperList();
            ViewBag.Manager = _appointmentService.GetManagerList();
            ViewBag.Regular = _appointmentService.GetRegularList();


            var objLeave = leave.Select(y => new SelectListItem
            {
                Text = y.Name,
                Value = y.Id.ToString()
            });
            var obj = new CreateRequest()
            {
                LeaveTypes = objLeave
            };
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRequest m)
        {
            try
            {
                var leave = await _leaveType.FindAll();

                ViewBag.Employee = _appointmentService.GetEmployeeList();
                ViewBag.Supervisor = _appointmentService.GetSuperList();
                ViewBag.Manager = _appointmentService.GetManagerList();
                ViewBag.Regular = _appointmentService.GetRegularList();

                var objLeave = leave.Select(y => new SelectListItem
                {
                    Text = y.Name,
                    Value = y.Id.ToString()
                });
                var obj = new CreateRequest()
                {
                    LeaveTypes = objLeave
                };
                m.LeaveTypes = objLeave;

                if (ModelState.IsValid == false) return View(m);


                if (DateTime.Compare(Convert.ToDateTime(m.StartDate), Convert.ToDateTime(m.EndDate)) > 1)
                {
                    ModelState.AddModelError("", "Start Date must be greater than End Date");
                    return View(m);
                }



                var user = _userManager.GetUserAsync(User).Result;

                var allocation = await _leaveAllocation.GetUserIdWithTypes(user.Id, m.LeaveTypeId);

                int daysRequested = (int)(Convert.ToDateTime(m.EndDate) - Convert.ToDateTime(m.StartDate)).TotalDays;

                if (allocation == null)
                    ModelState.AddModelError("", "You Have No Days Left");
                if (daysRequested > allocation.NumberOfDays)
                    ModelState.AddModelError("", "You Do Not Sufficient Days For This Request");

                var requestObj = new LeaveRequestVM()
                {
                    StartDate = Convert.ToDateTime(m.StartDate),
                    EndDate = Convert.ToDateTime(m.EndDate),
                    DateRequested = DateTime.Now,
                    RequestComments = m.RequestComments,
                    RequestingEmployeeId = user.Id,
                    LeaveTypeId = m.LeaveTypeId,
                    Approved = null,
                    Cancelled = true,
                    SuperVisorId = m.SuperVisorId,
                    ManagerId = m.ManagerId,
                    RegularId = m.RegularId,
                };

                var objMap = _mapper.Map<LeaveRequest>(requestObj);

                /*int count = request.Count(i => i.Approved == null);*/

                var pendReq = await _leaveRequest.CheckRequest(user.Id);

                int pendingCount = pendReq.Count(i => i.Approved == null);

                int approvedCount = pendReq.Count(i => i.Approved == true && i.Cancelled == false);

                if (pendingCount > 2)
                {
                    ModelState.AddModelError("", "Pending request must not greater than to two");
                    return View(m);
                }
                if(approvedCount >= 1)
                {
                    ModelState.AddModelError("", "Once your request is approved. It takes month to be able to request again.");
                    return View(m);
                }
                var success = await _leaveRequest.Create(objMap);
                 
                if (success == false)
                {
                    ModelState.AddModelError("", "Error Creation");

                    return View("Index");
                }
                return RedirectToAction("Index");

            }
            catch (Exception)
            {
/*                var dropdo = await _leaveRequest.RequestDropdown();
                ViewBag.leaveName = new SelectList(dropdo.LeaveTypes, "Id", "Name");
*/
                ModelState.AddModelError("", "Wrong o");
                return View(m);

            }
        }


        public async Task<IActionResult> Details(int id)
        {
            var request = await _leaveRequest.FindById(id);

            var obj = _mapper.Map<LeaveRequestVM>(request);
            return View(obj);
        }
        public async Task<IActionResult> RejectRequest(int id)
        {
            var request = await _leaveRequest.FindById(id);
            var user = _userManager.GetUserAsync(User).Result;

            request.Cancelled = false;
            request.ApprovedById = user.Id;


            request.Approved = false;
            request.DateActioned = DateTime.Now;
            await _leaveRequest.Update(request);
            return RedirectToAction("Index");

        }
        public async Task<IActionResult> ApproveRequest(int id)
        {
            var user = _userManager.GetUserAsync(User).Result;
            var request = await _leaveRequest.FindById(id);

            var allocation = await _leaveAllocation.GetUserIdWithTypes(request.RequestingEmployeeId, request.LeaveTypeId);

            int daysRequested = (int)(request.EndDate - request.StartDate).TotalDays;
            if (daysRequested > allocation.NumberOfDays)
            {
                ModelState.AddModelError("", "Error daysrequested");
                return RedirectToAction("Index");
            }
            allocation.NumberOfDays -= daysRequested;

            request.Cancelled = false;
            request.ApprovedById = user.Id;
            request.Approved = true;
            request.Count -= 1;
            request.DateActioned = DateTime.Now;
            await _leaveRequest.Update(request);
            return RedirectToAction("Index");
             
        }

        public async Task<IActionResult> CancelRequest(int id)
        {
            var request = await _leaveRequest.FindById(id);
            
            await _leaveRequest.Delete(request);

            return RedirectToAction("MyLeave");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var obj = await _leaveRequest.FindById(id);


            var dropdo = await _leaveRequest.RequestDropdown();
            ViewBag.leaveName = new SelectList(dropdo.LeaveTypes, "Id", "Name");

            ViewBag.Employee = _appointmentService.GetEmployeeList();
            ViewBag.Supervisor = _appointmentService.GetSuperList();
            ViewBag.Manager = _appointmentService.GetManagerList();
            ViewBag.Regular = _appointmentService.GetRegularList();

            /*      var selectList = obj.Select(i => new SelectListItem
                  {
                      Text = i.Name,
                      Value = i.Id.ToString()
                  });*/

            var map = _mapper.Map<EditRequest>(obj);

            return View(map);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeaveRequestVM m)
        {
            
                var request = await _leaveRequest.FindById(m.Id);

                var dropdo = await _leaveRequest.RequestDropdown();
                ViewBag.leaveName = new SelectList(dropdo.LeaveTypes, "Id", "Name");

                ViewBag.Supervisor = _appointmentService.GetSuperList();
                ViewBag.Manager = _appointmentService.GetManagerList();
                ViewBag.Regular = _appointmentService.GetRegularList();

                request.EndDate = m.EndDate;
                request.LeaveTypeId = m.LeaveTypeId;
                request.StartDate = m.StartDate;
                request.RequestComments = m.RequestComments;
                request.SuperVisorId = m.SuperVisorId;
              
            

                await _leaveRequest.Update(request);
                return RedirectToAction("MyLeave");



        
        }
        public async Task<IActionResult> RejectedIndex()
        {
            var user = _userManager.GetUserAsync(User).Result;


            var request = await _leaveRequest.FindAll();

            var requestObj = _mapper.Map<List<LeaveRequestVM>>(request);

            var obj = new RequestCountVM()
            {
                LeaveRequests = requestObj.Where(i=> i.Approved == false && i.Cancelled==false).ToList()
            };

            return View(obj);
        }

        public async Task<IActionResult> ApproveIndex()
        {
            var user = _userManager.GetUserAsync(User).Result;


            var request = await _leaveRequest.FindAll();

            var requestObj = _mapper.Map<List<LeaveRequestVM>>(request);

            var obj = new RequestCountVM()
            {
                LeaveRequests = requestObj.Where(i => i.Approved == true && i.Cancelled == false).ToList()
            };

            return View(obj);
        }

        public async Task<IActionResult> PendingIndex()
        {
            var user = _userManager.GetUserAsync(User).Result;

            // TODO: @LEAVEALLOCATION INDEX I NEED TO CREATE SUPERVISOR EMPLOYEE/REGULAR/MANAGER THAT 
            // POPULATE EACH ROLE

            var request = await _leaveRequest.FindAll();

            var requestObj = _mapper.Map<List<LeaveRequestVM>>(request);

            var obj = new RequestCountVM()
            {
                LeaveRequests = requestObj.Where(i => i.Approved == null && i.Cancelled == true).ToList()
            };

            return View(obj);
        }



        public async Task<IActionResult> TotalEmployees()
        {
            var request = await _leaveRequest.FindAll();
            var objRequest = _mapper.Map<List<LeaveRequestVM>>(request);

            var obj = new RequestCountVM()
            {
                LeaveRequests = objRequest
            };
            return View(obj);
        }

    }

}
