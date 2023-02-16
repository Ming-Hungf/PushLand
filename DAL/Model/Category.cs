using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Category:MainBaseClass
    {
        public string Name { get; set; }
        public int Sort { get; set; }
        public int? ParentID { get; set; }
        public ICollection<Category>? Children { get; set; }
        public Category? Parent { get; set; }
    }
}
