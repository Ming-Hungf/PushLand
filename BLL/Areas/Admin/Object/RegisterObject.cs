using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class RegisterObject
    {
        public string Email { get; set; }
        public string Phone { get; set; }
        public int ProjectID { get; set; }
        public List<int> InformationIDs { get; set; }
    }
}
