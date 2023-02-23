using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.ViewModel
{
    public class LoginViewModel
    {
        public string Phone { get; set; }
        public string Password { get; set; }
        public int Remember { get; set; }
        public string DeviceToken { get; set; }
    }
}
