using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;
using HrMan.Models;

namespace HrMan.Mapper
{
    public class Map : Profile
    {
        public Map()
        {
            CreateMap<LeaveType, LeaveTypeVM>().ReverseMap();
            
            CreateMap<LeaveRequest, LeaveRequestVM>().ReverseMap();
            CreateMap<LeaveRequest, AllocationIndex>().ReverseMap();
            CreateMap<LeaveRequest, EditRequest>().ReverseMap();


            CreateMap<LeaveAllocation, LeaveAllocationVM>().ReverseMap();
            CreateMap<LeaveAllocation, SetLeaveVM>().ReverseMap();
            /*           CreateMap<LeaveAllocation, EditLeaveAllocationVM>().ReverseMap();*/

            CreateMap<Employee, EmployeeVM>().ReverseMap();
            CreateMap<Employee, EmployeeVMTwo>().ReverseMap();
            CreateMap<Employee, EmployeeIndexVM>().ReverseMap();

            CreateMap<Attendance, AttendanceVM>().ReverseMap();
            CreateMap<Attendance, EmployeeVMTwo>().ReverseMap();


            CreateMap<PaymentRecord, IndexRecord>().ReverseMap();
            CreateMap<PaymentRecord, PaymentRecordVM>().ReverseMap();
            CreateMap<PaymentRecord, CreateRecord>().ReverseMap();

        }
    }
}
