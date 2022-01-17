using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HrMan.Contract;
using HrMan.Data;

namespace HrMan.Repository
{
    public class TaxRepo: ITax
    {
        private decimal taxRate;
        private decimal tax;

        private readonly ApplicationDbContext _context;
        public TaxRepo(ApplicationDbContext db)
        {
            _context = db;
        }

        public IEnumerable<TaxYear> FindAllTax()
        {
            return _context.TaxYears.ToList();
        }

        public decimal TaxAmount(decimal totalAmount)
        {
            // UK montlhy allowance // 1042
            if (totalAmount <= 1042)
            {
                taxRate = 0.0m;
                tax = totalAmount * taxRate;
            }
            else if (totalAmount > 1042 && totalAmount <= 3125)
            {
                //Basic tax rate
                taxRate = .20m;
                //Income tax
                tax = (1042 * .0m) + ((totalAmount - 1042) * taxRate);
            }
            else if (totalAmount > 3125 && totalAmount <= 12500)
            {
                //Higher tax rate
                taxRate = .40m;
                //Income tax
                tax = (1042 * .0m) + ((3125 - 1042) * .20m) + ((totalAmount - 3125) * taxRate);
            }
            else if (totalAmount > 12500)
            {
                //Additional tax Rate
                taxRate = .45m;
                //Income tax
                tax = (1042 * .0m) + ((3125 - 1042) * .20m) +
                    ((12500 - 3125) * .40m) + ((totalAmount - 12500) * taxRate);
            }
            return tax;
        }

    }
}
