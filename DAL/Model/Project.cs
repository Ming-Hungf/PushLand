using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Project:MainBaseClass
    {
        public string Title { get; set; }
        public float Square { get; set; }
        public int Price { get; set;}
        public int TotalPrice { get; set; }
        public int WardID { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int Bathroom { get; set; }
        public int Bedroom { get; set; }
        public string MainDirection { get; set; }
        public string BalconyDirection { get; set; }
        public int EstateType { get; set; }
        public int TransactionType { get; set; }
        public int CategoryID { get; set; }
        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public int Validate { get; set; }
        public IEnumerable<Project_Document> Documents { get; set; }
    }
}
