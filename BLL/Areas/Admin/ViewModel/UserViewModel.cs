using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Areas.Admin.Object
{
    public class UserViewModel
    {
        public int ID { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Birth { get; set; }
        public int Sex { get; set; }
        public string Image { get; set; }
        public int RoleID { get; set; }
        public string UserCreated { get; set; }
        public string UserUpdated { get; set; }
        public UserViewModel(User user)
        {
            ID = user.ID;
            Fullname = user.Fullname;
            Phone = user.Phone;
            Email = user.Email;
            Birth = user.Birth;
            Sex = user.Sex;
            Image = user.Image;
            RoleID = user.RoleID;
        }
        public UserViewModel() { }
    }
}
