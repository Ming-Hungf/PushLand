using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class SliderObject
    {
        public int ID { get; set; }
        public string Image { get; set; }
        public IFormFile File { get; set; }
        public long Ticks { get; set; }
        public int Sort { get; set; }
    }
}
