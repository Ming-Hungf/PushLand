using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class User:MainBaseClass
    {
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Birth { get; set; }
        public int Sex { get; set;}
        public string Image { get; set; }
        public int IsAdmin { get; set; }
        public int RoleID { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPCreated { get; set; }
        public string DeviceToken { get; set; }
    }
}
