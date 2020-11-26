using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webApi.Entitys;

namespace webApi.Models
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Gender GenderDisplay { get; set; }
        public int age { get; set; }
    }
}
