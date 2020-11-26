using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApi.Models
{
    //这个是面向用户的
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string CompanyName { get; set; }
    }
}
