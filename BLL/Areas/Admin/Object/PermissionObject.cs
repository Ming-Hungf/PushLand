using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class PermissionObject
    {
        public string? Controller { get; set; }
        public List<Function>? Actions { get; set; }
    }
}
