using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class RoleObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public RoleObject(Role role)
        {
            ID = role.ID;
            Name = role.Name;
        }
    }
}
