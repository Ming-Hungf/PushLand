using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Customer_Information:BaseClass
    {
        public int CustomerID { get; set; }
        public int ProjectID { get; set; }
        public int InformationID { get; set; }
    }
}
