using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;
using HrMan.Utility;
using System.Collections;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace HrMan.Controllers
{
    [Authorize(Roles = "Admin,Administrator")]
    public class LeaveAllocationController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IEmployee _employee;
        private readonly ILeaveType _leaveType;
        private readonly ILeaveRequest _leaveRequest;
        private readonly ILeaveAllocation _leaveAllocation;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        public LeaveAllocationController(ILeaveType leaveType,
                IMapper map, ILeaveAllocation leaveAllocation, IEmployee employee, ILeaveRequest leaveRequest,
                IAppointmentService appointmentService, IWebHostEnvironment webHostEnvironment,
                UserManager<Employee> userManage)
        {
            _hostingEnvironment = webHostEnvironment;
            _leaveRequest = leaveRequest;
            _leaveType = leaveType;
            _mapper = map;
            _leaveAllocation = leaveAllocation;
            _userManager = userManage;
            _appointmentService = appointmentService;
            _employee = employee;
        }

        public async Task<IActionResult> Index(int? pageNumber)
        {
            /* var empList = new { };*/
            await _leaveAllocation.FindAll();



            var emp = await _employee.FindAll();

            var user = _userManager.GetUsersInRoleAsync(Helper.Regular).Result;
            /*    var user = _userManager.GetUserAsync(User).Result;*/

            var empList = _mapper.Map<List<EmployeeIndexVM>>(emp).ToList();


          /*  empList = empList.Where(i => i.Role.Equals("SuperVisor")).ToList();
            empList = empList.Where(i => i.Role.Equals("Manager")).ToList();*/
            /* var empList = _mapper.Map<List<EmployeeVM>>(emp).ToList();*/
            var request = _mapper.Map<List<LeaveRequestVM>>(await _leaveRequest.FindAll());

/*
            var obj = new EmpRoleIndexVM()
            {
                LeaveRequests = request.ToList(),
                Employees = empList.ToList()
            };*/

            int pageSize = 2;



            return View(Pagination<EmployeeIndexVM>.Create(empList, pageNumber ?? 1, pageSize));
        }
        public async Task<IActionResult> SecIndex()
        {
            var emp = await _employee.FindAll();

            var user = _userManager.GetUsersInRoleAsync("SuperVisor").Result;

            var empList = _mapper.Map<List<EmployeeIndexVM>>(emp).ToList();

            return View(empList);
        }
        public async Task<IActionResult> Filter(string searchString)
        {

            var emp = await _employee.FindAll();

            var user = _userManager.GetUsersInRoleAsync("SuperVisor").Result;

            var empList = _mapper.Map<List<EmployeeIndexVM>>(emp).ToList();


            if (string.IsNullOrEmpty(searchString) == false)
            {
                var filter = empList.Where(i => i.Firstname.ToLower().Contains(searchString));


                return View("SecIndex", filter);
            }
            return View("SecIndex", empList);
        }


        public async Task<IActionResult> SetAllocation()
        {
            var emp = _mapper.Map<List<LeaveType>>(await _leaveType.FindAll());

            var empList = await _employee.FindAll();

            /*var user = _userManager.GetUsersInRoleAsync("Employee").Result;*/

            var empMap = _mapper.Map<List<EmployeeVMTwo>>(empList);

            var obj = new AllocationIndex()

            {
                LeaveTypes = emp,
                userCount = empMap.Count(i => i.newUser > 0)
            };

            return View(obj);
        }

        public async Task<IActionResult> SetLeave(int id)
        {
            var leave = await _leaveType.FindById(id);
            // getting all user except admin
            /*var user = _userManager.GetUsersInRoleAsync("Employee").Result;
*/
            var emp = await _employee.FindAll();

            if (ModelState.IsValid)
            {
                foreach (var item in emp)
                {
                    if (await _leaveAllocation.CheckAllocation(item.Id, leave.Id))
                        continue;

                    var obj = new SetLeaveVM()
                    {
                        NumberOfDays = leave.DefaultDays,
                        LeaveTypeId = leave.Id,
                        EmployeeId = item.Id,
                        DateCreated = DateTime.Now,
                        Period = DateTime.Now.Year
                    };
             
                    var objMap = _mapper.Map<LeaveAllocation>(obj);
                    await _leaveAllocation.Create(objMap);
                }

            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("SetAllocation");
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = _mapper.Map<EmployeeVMTwo>(_userManager.FindByIdAsync(id).Result);

            var allocation = _mapper.Map<List<LeaveAllocationVM>>(await _leaveAllocation.GetAllocationWithUser(id));


            var obj = new DetailsAllocationVM()
            {
                Employees = user,
                LeaveAllocations = allocation
            };

            return View(obj);
        }

        public IActionResult Edit(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

        /*    var emp = new EmployeeVMTwo()
            {
                contactNumber = user.contactNumber,
                DateJoined = user.DateJoined,
                DateOfBirth = user.DateOfBirth,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
            };*/


            var userList = _mapper.Map<EmployeeVMTwo>(user);

            
            return View(userList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeVM m)
        {
            if (ModelState.IsValid)
            {

                var obj = _userManager.FindByIdAsync(m.Id).Result;
           
                obj.Email = m.Email;
                obj.Firstname = m.Firstname;
                obj.Lastname = m.Lastname;
                obj.DateJoined = m.DateJoined;
                obj.DateOfBirth = m.DateOfBirth;
                obj.contactNumber = m.contactNumber;

                if (obj.ImageUrl == null || obj.ImageUrl != null)

                {
                    var uploadDir = @"images/employees";
                    var fileName = Path.GetFileNameWithoutExtension(m.ImageUrl.FileName);
                    var extension = Path.GetExtension(m.ImageUrl.FileName);
                    var webRootPath = _hostingEnvironment.WebRootPath;
                    fileName = DateTime.UtcNow.ToString("yymmssfff") + fileName + extension;

                    var path = Path.Combine(webRootPath, uploadDir, fileName);
                    await m.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                   /* await d.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));*/
                    obj.ImageUrl = "/" + uploadDir + "/" + fileName;
                }

                 await _employee.Update(obj);
               
                return RedirectToAction("Index");
            }
           

            return View(m);
        }
    
        public async Task<IActionResult> EmployeeSuperVisor()
        {

            var emp = await _employee.FindAll();

            var empList = _mapper.Map<List<EmployeeVMTwo>>(emp);

            var obj = new EmpRoleIndexVM()
            {
                Employees = empList.Where(i => i.Role.Equals("SuperVisor")).ToList()
            };

            return View(obj);
        }
        public async Task<IActionResult> EmployeeManager()
        {
            var emp = await _employee.FindAll();

            var empList = _mapper.Map<List<EmployeeVMTwo>>(emp);

            var obj = new EmpRoleIndexVM()
            {
                Employees = empList.Where(i => i.Role.Equals("Manager")).ToList()
            };

            return View(obj);
        }
    }
}
