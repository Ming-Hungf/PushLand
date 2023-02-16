using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class News : MainBaseClass
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<News_Image> Images { get; set; }
    }
}
