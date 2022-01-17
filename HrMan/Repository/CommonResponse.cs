using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HrMan.Repository
{
    public class CommonResponse<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T dataenum { get; set; }
    }
}
