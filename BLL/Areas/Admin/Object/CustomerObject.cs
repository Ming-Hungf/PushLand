using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class CustomerObject
    {
        public int ID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<InformationObject> Informations { get; set; }
        public int Status { get; set; }
    }
}
