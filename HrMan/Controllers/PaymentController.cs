using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RotativaCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;
using Microsoft.AspNetCore.Identity;

namespace HrMan.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentRecord _paymentRecord;
        private readonly ITax _tax;
        private readonly IMapper _mapper;
        private readonly UserManager<Employee> _userManager;

        public PaymentController(IPaymentRecord paymentRecord, UserManager<Employee> user,ITax tax, IMapper map)
        {
            _paymentRecord = paymentRecord;
            _tax = tax;
            _mapper = map;
            _userManager = user;
        }

        decimal overTimeHours;
        decimal overTimeEarnings;
        decimal contractualEarnings;

        decimal tax;
        decimal totalEarnings;
        decimal totalDeductions;

        public async Task<IActionResult> Index()
        {
            var objMap = await _paymentRecord.ListOfPayment();

            var obj = _mapper.Map<List<IndexRecord>>(objMap);

            /* var obj = _paymentRecord.ListOfPayment().Select(e => new IndexRecord
             {
                 Id = e.Id,
                 PayDate = e.PayDate,
                 PayMonth = e.PayMonth,
                 TotalDeduction = e.TotalDeduction,
                 RequestingEmployee = e.RequestingEmployee,
                 RequestingEmployeeId = e.RequestingEmployeeId,
                 NetPayment = e.NetPayment,
                 TaxYearId = e.TaxYearId,
                 TotalEarnings = e.TotalEarnings,
                 Year = _paymentRecord.GetTaxYearId(e.TaxYearId).YearOfTax,
             });*/
            return View(obj);
        }

        public async Task<IActionResult> Create()
        {
            var dropdown = await _paymentRecord.PaymentDropDown();
            ViewBag.taxYears = new SelectList(dropdown.TaxYears, "Id", "YearOfTax");
            ViewBag.employees = new SelectList(dropdown.Employees, "Id", "Fullname");

            var payment = await _paymentRecord.ListOfPayment();

            var objPay = payment.Select(i => new SelectListItem
            {
                Text = i.Firstname,
                Value = i.Id.ToString()
            });
            var obj = new CreateRecord
            {
                RequestingEmployees = objPay
            };

            return View(obj);

        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRecord d)
        {
            var dropdown = await _paymentRecord.PaymentDropDown();
            ViewBag.employees = new SelectList(dropdown.Employees, "Id", "Fullname");
            ViewBag.taxYears = new SelectList(dropdown.TaxYears, "Id", "YearOfTax");

            var objMap = _mapper.Map<PaymentRecord>(d);

            objMap.RequestingEmployeeId = d.RequestingEmployeeId;
            objMap.OvertimeHours = overTimeHours = _paymentRecord.OvertimeHours(d.HoursWorked, d.ContractualHours);
            objMap.ContractualEarnings = contractualEarnings = _paymentRecord.ContractualEarnings(d.ContractualHours, d.HoursWorked, d.HourlyRate);
            objMap.OvertimeEarnings = overTimeEarnings = _paymentRecord.OvertimeEarnings(_paymentRecord.OvertimeRate(d.HourlyRate), d.OvertimeHours);
            objMap.TotalEarnings = totalEarnings = _paymentRecord.TotalEarnings(overTimeEarnings, contractualEarnings);
            objMap.Tax = tax = _tax.TaxAmount(totalEarnings);
            objMap.TotalDeduction = totalDeductions = _paymentRecord.TotalDeduction(tax);
            objMap.NetPayment = _paymentRecord.NetPay(totalEarnings, totalDeductions);

            await _paymentRecord.AddPayment(objMap);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {

            var dropdown = await _paymentRecord.PaymentDropDown();
            ViewBag.taxYears = new SelectList(dropdown.TaxYears, "Id", "YearOfTax");
            ViewBag.employees = new SelectList(dropdown.Employees, "Id", "Fullname");

            var payment = await _paymentRecord.ListOfPayment();

            var objPay = payment.Select(i => new SelectListItem
            {
                Text = i.Firstname,
                Value = i.Id.ToString()
            });
            var h = await _paymentRecord.GetPaymentId(id);

            var obj = _mapper.Map<CreateRecord>(h);
            obj.Year = _paymentRecord.GetTaxYearId(h.TaxYearId).YearOfTax;


            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateRecord d)
        {
            var dropdown = await _paymentRecord.PaymentDropDown();
            ViewBag.employees = new SelectList(dropdown.Employees, "Id", "Fullname");
            ViewBag.taxYears = new SelectList(dropdown.TaxYears, "Id", "YearOfTax");
            try
            {
                if (ModelState.IsValid == false)
                {
                    ModelState.AddModelError("", "Model Error");
                    return View(d);
                }
                var objMap = await _paymentRecord.GetPaymentId(d.Id);

                objMap.PayDate = d.PayDate;
                objMap.PayMonth = d.PayMonth;
                objMap.OvertimeHours = overTimeHours = _paymentRecord.OvertimeHours(d.HoursWorked, d.ContractualHours);
                objMap.ContractualEarnings = contractualEarnings = _paymentRecord.ContractualEarnings(d.ContractualHours, d.HoursWorked, d.HourlyRate);
                objMap.OvertimeEarnings = overTimeEarnings = _paymentRecord.OvertimeEarnings(_paymentRecord.OvertimeRate(d.HourlyRate), d.OvertimeHours);
                objMap.TotalEarnings = totalEarnings = _paymentRecord.TotalEarnings(overTimeEarnings, contractualEarnings);
                objMap.Tax = tax = _tax.TaxAmount(totalEarnings);
                objMap.TotalDeduction = totalDeductions = _paymentRecord.TotalDeduction(tax);
                objMap.NetPayment = _paymentRecord.NetPay(totalEarnings, totalDeductions);

                var success = _paymentRecord.UpdaatePayment(objMap);
                if (!success)
                {
                    ModelState.AddModelError("", "Failed");
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error");
                return RedirectToAction("Index");

            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var dropdown = await _paymentRecord.PaymentDropDown();
            ViewBag.taxYears = new SelectList(dropdown.TaxYears, "Id", "YearOfTax");
            ViewBag.employees = new SelectList(dropdown.Employees, "Id", "Fullname");

            var payment = await _paymentRecord.ListOfPayment();

            var objPay = payment.Select(i => new SelectListItem
            {
                Text = i.Firstname,
                Value = i.Id.ToString()
            });
            var h = await _paymentRecord.GetPaymentId(id);

            var obj = _mapper.Map<CreateRecord>(h);
            obj.Year = _paymentRecord.GetTaxYearId(h.TaxYearId).YearOfTax;


            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(CreateRecord d)
        {
            var dropdown = await _paymentRecord.PaymentDropDown();
            ViewBag.employees = new SelectList(dropdown.Employees, "Id", "Fullname");
            ViewBag.taxYears = new SelectList(dropdown.TaxYears, "Id", "YearOfTax");

            if (ModelState.IsValid == false)
            {
                ModelState.AddModelError("", "Model Error");
                return View(d);
            }
            var objMap = await _paymentRecord.GetPaymentId(d.Id);

            objMap.PayDate = d.PayDate;
            objMap.PayMonth = d.PayMonth;
            objMap.OvertimeHours = overTimeHours = _paymentRecord.OvertimeHours(d.HoursWorked, d.ContractualHours);
            objMap.ContractualEarnings = contractualEarnings = _paymentRecord.ContractualEarnings(d.ContractualHours, d.HoursWorked, d.HourlyRate);
            objMap.OvertimeEarnings = overTimeEarnings = _paymentRecord.OvertimeEarnings(_paymentRecord.OvertimeRate(d.HourlyRate), d.OvertimeHours);
            objMap.TotalEarnings = totalEarnings = _paymentRecord.TotalEarnings(overTimeEarnings, contractualEarnings);
            objMap.Tax = tax = _tax.TaxAmount(totalEarnings);
            objMap.TotalDeduction = totalDeductions = _paymentRecord.TotalDeduction(tax);
            objMap.NetPayment = _paymentRecord.NetPay(totalEarnings, totalDeductions);

            await _paymentRecord.RemovePayment(objMap);
            return RedirectToAction("Index"); ;
        }

        public async Task<IActionResult> Details(int id)
        {
            var h = await _paymentRecord.GetPaymentId(id);
            var obj = _mapper.Map<CreateRecord>(h);

            obj.Year = _paymentRecord.GetTaxYearId(h.TaxYearId).YearOfTax;
            /*  var obj = new CreateRecord()
              {
                  Id = h.Id,
                  RequestingEmployeeId = h.RequestingEmployeeId,
                  PayMonth = h.PayMonth,
                  PayDate = h.PayDate,
                  TaxYearId = h.TaxYearId,
                  TaxCode = h.TaxCode,
                  HourlyRate = h.HourlyRate,
                  HoursWorked = h.HoursWorked,
                  ContractualEarnings = h.ContractualEarnings,
                  ContractualHours = h.ContractualHours,
                  OvertimeHours = h.OvertimeHours,
                  OvertimeEarnings = h.OvertimeEarnings,
                  TotalDeduction = h.TotalDeduction,
                  TotalEarnings = h.TotalEarnings,
                  Tax = h.Tax,
                  NetPayment = h.NetPayment,
                  Firstname=h.Firstname,
                  Lastname=h.Lastname,
                  RequestingEmployee = h.RequestingEmployee,
                  Year = _paymentRecord.GetTaxYearId(h.TaxYearId).YearOfTax,
                  OvertimeRate = _paymentRecord.OvertimeRate(h.HoursWorked)
              };*/

            return View(obj);
        }

        public async Task<IActionResult> Payslip(int id)
        {
            var h = await _paymentRecord.GetPaymentId(id);
            var obj = _mapper.Map<CreateRecord>(h);

            obj.Year = _paymentRecord.GetTaxYearId(h.TaxYearId).YearOfTax;

            return View(obj);
        }
        public IActionResult GeneratePaySlip(int id)
        {
            var pdf = new ActionAsPdf("PaySlip", new { id = id })
            {
                FileName = "payslip.pdf"
            };

            return pdf;
        }

        public async Task<IActionResult> MyPayroll(PaymentRecordVM h)
        {
            var user = _userManager.GetUserAsync(User).Result;

            var payment = _mapper.Map<List<PaymentRecordVM>>(await _paymentRecord.GetPayrollByEmployeeId(user.Id));


            /*payment.Select(i => i.Year == _paymentRecord.GetTaxYearId(h.TaxYearId).YearOfTax);
            var tax = _tax.FindAllTax();

            var objTax = _mapper.Map<TaxYearVM>(tax);

            objTax.Year = _paymentRecord.GetTaxYearId(h.TaxYearId).YearOfTax;*/

            var obj = new MyPaymentRecordVM()
            {
                PaymentRecords = payment,
            };
            
            return View(obj);
        }
    }
}
