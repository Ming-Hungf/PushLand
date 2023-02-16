using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class UserSession:BaseClass
    {
        public string AccessToken { get; set; }
        public int LoginResult { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string TypeID { get; set; }
        public int Remember { get; set; } 
    }
}
