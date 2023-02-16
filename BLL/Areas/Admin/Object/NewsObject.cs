using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class NewsObject
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}
