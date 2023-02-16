using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Object
{
    public class ChangePasswordRequest
    {
        public string Old { get; set; }
        public string New { get; set; }
    }
}
