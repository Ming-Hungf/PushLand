using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class CustomerInformationObject
    {
        public List<CustomerObject> Customers { get; set; }
        public List<InformationObject> Informations { get; set; }
    }
}
