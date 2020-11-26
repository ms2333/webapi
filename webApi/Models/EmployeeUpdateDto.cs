using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApi.Entitys;

namespace webApi.Models
{
    public class EmployeeUpdateDto
    {
        public string FirstName { get; set; }
        public Gender GenderDisplay { get; set; }
        public int age { get; set; }
    }
}
