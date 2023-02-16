using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Config:BaseClass
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Body { get; set; }
        public string Favicon { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Facebook { get; set; }
        public string Youtube { get; set; }
        public string Insta { get; set; }
        public string Description { get; set; }
    }
}
