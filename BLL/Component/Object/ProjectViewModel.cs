using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Object
{
    public class ProjectViewModel
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public int TotalPrice { get; set; }
        public int Bathroom { get; set; }
        public int Bedroom { get; set; }
        public string MainDirect { get; set; }
        public string Balcony { get; set; }
        public float Square { get; set; }
        public int WardID { get; set; }
    }
}
