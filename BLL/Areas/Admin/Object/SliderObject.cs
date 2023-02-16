using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class SliderObject
    {
        public IFormFile File { get; set; }
        public long Ticks { get; set; }
    }
}
