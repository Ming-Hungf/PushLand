using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class ProjectDetails:BaseClass
    {
        public int ProjectID { get; set; }
        public string? ProjectName { get; set; }
        public string? Investor { get; set; }
        public int? Elevator { get; set; }
        public int? Floor { get; set; }
        public int? TotalFloor { get; set; }
        public DateTime? TransferDate { get; set; }

    }
}
