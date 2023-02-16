using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Information:MainBaseClass
    {
        public string Name { get; set; }
        public List<Customer> Customer { get; set; }
    }
}
