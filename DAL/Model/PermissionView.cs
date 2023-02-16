using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    [Keyless]
    public class PermissionView
    {
        public int RoleID { get; set; }
        public int FunctionID { get; set; }
        public string FunctionName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
