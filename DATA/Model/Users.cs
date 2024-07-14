using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.Model
{
    public class Users
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }

    }
}
