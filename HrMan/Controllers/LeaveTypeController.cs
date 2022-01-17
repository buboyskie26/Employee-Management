using AutoMapper;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Controllers
{
    [Authorize]
    public class LeaveTypeController : Controller
    {
        private readonly ILeaveType _leaveType;
        private readonly IMapper _mapper;
        public LeaveTypeController(ILeaveType leaveType, IMapper map)
        {
            _leaveType = leaveType;
            _mapper = map;
        }
        public async Task<IActionResult> Index()
        {
            var leaveType = _mapper.Map<List<LeaveTypeVM>>(await _leaveType.FindAll());

            return View(leaveType);
        }
        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                if (ModelState.IsValid == false) return View(leaveTypeVM);

                var pbjMap = _mapper.Map<LeaveType>(leaveTypeVM);
                pbjMap.DateCreated = DateTime.Now;

                await _leaveType.Create(pbjMap);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something wrong");
                return RedirectToAction("Index");
            }

        }
        public async Task<IActionResult> Details(int id)
        {
            if (await _leaveType.isExists(id) == false) return NotFound();

            var leaveTypeId = await _leaveType.FindById(id);

            var objMap = _mapper.Map<LeaveTypeVM>(leaveTypeId);

            return View(objMap);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (await _leaveType.isExists(id) == false) return NotFound();

            var leaveTypeId = await _leaveType.FindById(id);

            var objMap = _mapper.Map<LeaveTypeVM>(leaveTypeId);

            return View(objMap);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                if (ModelState.IsValid == false) return View(leaveTypeVM);

                var pbjMap = _mapper.Map<LeaveType>(leaveTypeVM);

                await _leaveType.Update(pbjMap);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something wrong");
                return RedirectToAction("Index");
            }
        }


        public async Task<IActionResult> Delete(LeaveTypeVM leaveTypeVM)
        {
            try
            {
                if (ModelState.IsValid == false) return View(leaveTypeVM);

                var pbjMap = _mapper.Map<LeaveType>(leaveTypeVM);

                await _leaveType.Delete(pbjMap);
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Something wrong");
                return RedirectToAction("Index");
            }
        }
    }
}
