using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class InformationObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public InformationObject(Information information)
        {
            ID = information.ID;
            Name = information.Name;
        }
        public InformationObject()
        {

        }
    }
}
