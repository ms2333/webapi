using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webApi.Models
{
    public class CompanyParameters
    {
        public Guid CompanyId{ get; set; }
        public int PageNumber { get; set; } = 1;//default:1
        public int PageSize { get; set; }
        public string searchTerm { get; set; }

        //Orderby
        public string orderBy { set; get; } = "CompanyId";
    }
}
