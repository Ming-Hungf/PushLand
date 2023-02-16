using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.ViewModel
{
    public class ValidateViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string UserCreated { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int Validate { get; set; }
        public List<string> Images { get; set; }
    }
}
