using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;
using HrMan.Repository;
using HrMan.Utility;

namespace HrMan.Controllers.Api
{
    [Route("api/LeaveRequest")]
    [ApiController]
    public class LeaveRequestApiController : Controller
    {

        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginUserId;
        private readonly string role;


 
        public LeaveRequestApiController(IAppointmentService appointmentService,
            IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);

        }

        [HttpPost]
        [Route("SaveCalendarData")]
        public IActionResult SaveCalendarData(LeaveRequestVM m)
        {
            
            CommonResponse<int> commonResponse = new CommonResponse<int>();
            try
            {

                
                commonResponse.status = _appointmentService.AddUpdate(m).Result;



                if (commonResponse.status == 1)
                {
                    commonResponse.message = Helper.appointmentUpdated;
                }
                if (commonResponse.status == 2)
                {
                    commonResponse.message = Helper.appointmentAdded;
                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }

            return Ok(commonResponse);
        }


        [HttpGet]
        [Route("GetCalendarData")]
        public IActionResult GetCalendarData(string superVisorId)
        {

            CommonResponse<List<LeaveRequestVM>> commonResponse = new CommonResponse<List<LeaveRequestVM>>();
            try
            {
                if(role == Helper.SuperVisor)
                {
                    commonResponse.dataenum = _appointmentService.SuperVisorEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;


                }
                else if(role == Helper.Manager)
                {
                    commonResponse.dataenum = _appointmentService.ManagerEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;

                }
                else if (role == Helper.Regular)
                {
                    commonResponse.dataenum = _appointmentService.RegularEventsById(loginUserId);
                    commonResponse.status = Helper.success_code;

                }
                else
                {
                    commonResponse.dataenum = _appointmentService.SuperVisorEvents();
                    commonResponse.status = Helper.success_code;

                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
                
                return Ok(commonResponse);
        }

        [HttpGet]
        [Route("GetCalendarDataById/{id}")]
        public IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<LeaveRequestVM> commonResponse = new CommonResponse<LeaveRequestVM>();

            try
            {
                commonResponse.dataenum = _appointmentService.GetAllById(id);
                commonResponse.status = Helper.success_code;
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }
    }
}
