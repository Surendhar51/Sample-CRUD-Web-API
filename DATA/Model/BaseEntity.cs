using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Model
{
    public class BaseEntity
    {
        public bool IsActive { get; set; }
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifyOn { get; set; }
        public string ModifyBy { get; set; }
    }
}
