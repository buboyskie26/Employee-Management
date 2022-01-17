using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;
using HrMan.Models;

namespace HrMan.Repository
{
    public class PaymentRecordRepo : IPaymentRecord
    {
        private readonly ApplicationDbContext _context;
        public PaymentRecordRepo(ApplicationDbContext db)
        {
            _context = db;
        }

        private decimal overTimeHours;
        private decimal contractualEarnings;
        public async Task<PaymentRecord> GetPaymentId(int id)
        {
            return await _context.PaymentRecords
                 .Include(i => i.RequestingEmployee)
                 .Include(i => i.TaxYear)
                .FirstOrDefaultAsync(i => i.Id == id);

        }

        public TaxYear GetTaxYearId(int id)
        {
            return _context.TaxYears
                .FirstOrDefault(i => i.Id == id);
        }

        public async Task<IEnumerable<PaymentRecord>> ListOfPayment()
        {
            return await _context.PaymentRecords
                 .Include(i => i.RequestingEmployee)
                 .Include(i => i.TaxYear)
                 .OrderBy(i => i.Lastname).ToListAsync();
        }
        public async Task AddPayment(PaymentRecord paymentRecord)
        {
            await _context.PaymentRecords.AddAsync(paymentRecord);
            await _context.SaveChangesAsync();
        }
        public async Task RemovePayment(PaymentRecord paymentRecord)
        {
            _context.PaymentRecords.RemoveRange(paymentRecord);
            await _context.SaveChangesAsync();
        }
        public decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate)
        {
            if(hoursWorked < contractualHours)
            {
                contractualEarnings = hoursWorked * hourlyRate;
            }
            else
            {
               contractualEarnings = contractualHours* hourlyRate;
            }
            return contractualEarnings;
        }


        public decimal NetPay(decimal totalEarnings, decimal totalDeduction)
        {
            return totalEarnings - totalDeduction;

        }

        public decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours)
        {
            return overtimeRate + overtimeHours;
        }

        public decimal OvertimeHours(decimal hoursWorked, decimal contractualHours)
        {
            if (hoursWorked < contractualHours)
                overTimeHours = 0.00m;
            else
                overTimeHours = hoursWorked - contractualHours;

            return overTimeHours;
        }

        public decimal OvertimeRate(decimal hourlyRate)
        {
            return hourlyRate * 0.15m;
        }

        public async Task<DropDownVM> PaymentDropDown()
        {
            var obj = new DropDownVM()
            {
                Employees = await _context.Employees.Where(i =>i.UserName != "admin@gmail.com").OrderBy(i=> i.Fullname).ToListAsync(),
                TaxYears = await _context.TaxYears.OrderBy(i => i.YearOfTax).ToListAsync(),
            };

            return obj;
        }

    

        public decimal TotalDeduction(decimal tax )
        {
            return tax;
        }

        public decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings)
        {
            return overtimeEarnings + contractualEarnings;
        }

        public bool UpdaatePayment(PaymentRecord paymentRecord)
        {
             _context.PaymentRecords.Update(paymentRecord);
             return _context.SaveChanges() > 0;
        }

        public async Task<ICollection<PaymentRecord>> GetPayrollByEmployeeId(string id)
        {
            var obj = await ListOfPayment();
            return   obj.Where(i => i.RequestingEmployeeId == id).ToList();
        }
    }
}
