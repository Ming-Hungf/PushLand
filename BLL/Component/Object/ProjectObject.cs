using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Object
{
    public class ProjectObject:ProjectViewModel
    {
        public int Price { get; set; }
        public IEnumerable<IFormFile> Files { get; set; }
        public int TransactionType { get; set; }
        public int EstateType { get; set; }
        public int CategoryID { get; set; }
        public string? ProjectName { get; set; }
        public string? Investor { get; set; }
        public int? Elevator { get; set; }
        public int? Floor { get; set; }
        public int? TotalFloor { get; set; }
        public DateTime? TransferDate { get; set; }
        public string Description { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
    }
}
