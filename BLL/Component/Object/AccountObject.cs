using DAL.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Object
{
    public class AccountObject
    {
        
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Birth { get; set; }
        public int Sex { get; set; }
        public IFormFile File { get; set; }
        public string Image { get; set; }
        public AccountObject(User user)
        {
            Fullname = user.Fullname;
            Phone = user.Phone;
            Email = user.Email;
            Birth = user.Birth;
            Sex = user.Sex;
            Image = user.Image;
        }
        public AccountObject() { }
    }
}
