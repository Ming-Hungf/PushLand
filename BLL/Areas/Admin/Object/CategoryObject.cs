using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class CategoryObject
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ParentID { get; set; }
        public CategoryObject(Category category)
        {
            ID = category.ID;
            Name = category.Name;
            ParentID = category.ParentID;
        }
        public CategoryObject() { }
    }
}
