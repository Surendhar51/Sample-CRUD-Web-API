using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTILITY
{
    public class BadRequest
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Trace { get; set; }
    }
}
