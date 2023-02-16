using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Project_Document:BaseClass
    {
        public string Name { get; set;}
        public string Link { get; set; }
        public int ProjectID { get; set; }
    }
}
