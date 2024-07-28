using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTILITY
{
    public class Result
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public object Item { get; set; }
        public string ErrorMessage { get; set; }
    }
}
