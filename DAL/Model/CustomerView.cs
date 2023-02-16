using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    [Keyless]
    public class CustomerView
    {
        public int UserID { get; set; }
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int InformationID { get; set; }
        public string InformationName { get; set; }
        public int Status { get; set; }
    }
}
