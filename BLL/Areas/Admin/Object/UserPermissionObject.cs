using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class UserPermissionObject
    {
        public List<PermissionObject>? SystemFunctions { get; set; }
        public List<PermissionView>? UserPermissions { get; set; }
    }
}
