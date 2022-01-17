using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;
using HrMan.Models;

namespace HrMan.Contract
{
    public interface IPaymentRecord
    {
        Task<IEnumerable<PaymentRecord>> ListOfPayment();
        Task<PaymentRecord> GetPaymentId(int id);
        Task AddPayment(PaymentRecord paymentRecord);
        bool UpdaatePayment(PaymentRecord paymentRecord);
        Task RemovePayment(PaymentRecord paymentRecord);
        TaxYear GetTaxYearId(int id);
        decimal OvertimeHours(decimal hoursWorked, decimal contractualHours);
        decimal ContractualEarnings(decimal contractualHours, decimal hoursWorked, decimal hourlyRate);
        decimal OvertimeRate(decimal hourlyRate);
        decimal OvertimeEarnings(decimal overtimeRate, decimal overtimeHours);
        decimal TotalEarnings(decimal overtimeEarnings, decimal contractualEarnings);
        decimal TotalDeduction(decimal tax);
        decimal NetPay(decimal totalEarnings, decimal totalDeduction);
        Task<DropDownVM> PaymentDropDown();


        Task<ICollection<PaymentRecord>> GetPayrollByEmployeeId(string id);


    }
}
