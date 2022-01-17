using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Data;

namespace HrMan.Models
{
    public class PaymentRecordVM
    {
        public int Id { get; set; }
        public Employee RequestingEmployee { get; set; }
        public string RequestingEmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string NiNo { get; set; }
        public DateTime PayDate { get; set; }
        public string PayMonth { get; set; }
        public int TaxYearId { get; set; }
        public TaxYear TaxYear { get; set; }
        public string TaxCode { get; set; }
      
        public decimal HourlyRate { get; set; }
       
        public decimal HoursWorked { get; set; }
       
        public decimal ContractualHours { get; set; }
       
        public decimal OvertimeHours { get; set; }
  
        public decimal ContractualEarnings { get; set; }
  
        public decimal OvertimeEarnings { get; set; }
      
        public decimal Tax { get; set; }
      
        public decimal NIC { get; set; }
      
        public decimal? UnionFee { get; set; }
      
        public Nullable<decimal> SLC { get; set; }
      
        public decimal TotalEarnings { get; set; }
      
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }
        public string Year { get; set; }
        public decimal OvertimeRate { get; set; }
    }
    public class IndexRecord
    {
        public int Id { get; set; }
        public Employee RequestingEmployee { get; set; }
        public string RequestingEmployeeId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; }
        [Display(Name = "Month")]
        public string PayMonth { get; set; }
        public int TaxYearId { get; set; }
        public string Year { get; set; }
        [Display(Name = "Total Earnings")]
        public decimal TotalEarnings { get; set; }
        [Display(Name = "Total Deductions")]
        public decimal TotalDeduction { get; set; }
        [Display(Name = "Net")]
        public decimal NetPayment { get; set; }
    }
    public class MyPaymentRecordVM
    {
        public List<PaymentRecordVM> PaymentRecords { get; set; }
        public TaxYearVM TaxYears { get; set; }
    }
    public class CreateRecord
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]
        public Employee RequestingEmployee { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "Pay Month")]
        public string PayMonth { get; set; } = DateTime.Today.Month.ToString();
        [Display(Name = "Tax Year")]
        public int TaxYearId { get; set; }
        public TaxYear TaxYear { get; set; }
        public string TaxCode { get; set; } = "1250L";
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }
        [Display(Name = "Contractual Hours")]
        public decimal ContractualHours { get; set; } = 144m;
        public decimal OvertimeHours { get; set; }
        public decimal ContractualEarnings { get; set; }
        public decimal OvertimeEarnings { get; set; }
        public Nullable<decimal> SLC { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }
        public string RequestingEmployeeId { get; set; }
        public IEnumerable<SelectListItem> RequestingEmployees { get; set; }
        public string Year { get; set; }
        public decimal Tax { get; set; }
        public decimal OvertimeRate { get; set; }

    }


    public class DetailRecord
    {
        public int Id { get; set; }
        [Display(Name = "Full Name")]
        public Employee RequestingEmployee { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string NiNo { get; set; }
        [DataType(DataType.Date), Display(Name = "Pay Date")]
        public DateTime PayDate { get; set; } = DateTime.UtcNow;
        [Display(Name = "Pay Month")]
        public string PayMonth { get; set; } = DateTime.Today.Month.ToString();
        [Display(Name = "Tax Year")]
        public int TaxYearId { get; set; }
        public TaxYear TaxYear { get; set; }
        public string TaxCode { get; set; } = "1250L";
        [Display(Name = "Hourly Rate")]
        public decimal HourlyRate { get; set; }
        [Display(Name = "Hours Worked")]
        public decimal HoursWorked { get; set; }
        [Display(Name = "Contractual Hours")]
        public decimal ContractualHours { get; set; } = 144m;
        public decimal OvertimeHours { get; set; }
        public decimal ContractualEarnings { get; set; }
        public decimal OvertimeEarnings { get; set; }
        public decimal Tax { get; set; }
        public decimal NIC { get; set; }
        public decimal? UnionFee { get; set; }
        public Nullable<decimal> SLC { get; set; }
        public decimal TotalEarnings { get; set; }
        public decimal TotalDeduction { get; set; }
        public decimal NetPayment { get; set; }
        public string RequestingEmployeeId { get; set; }
    }

}
