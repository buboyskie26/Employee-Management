using HrMan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Contract
{
    public interface ITax
    {
        public decimal TaxAmount(decimal totalAmount);
        public IEnumerable<TaxYear> FindAllTax();
    }
}
