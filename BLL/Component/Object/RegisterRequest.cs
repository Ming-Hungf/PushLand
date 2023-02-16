using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Object
{
    public class RegisterRequest
    {
        public string Fullname { get; set; }
        public string Password { get; set;}
        public string Phone { get; set; }
        public string Email { get; set; }
        
    }
}
