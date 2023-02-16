using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class Notification:BaseClass
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int IsSeen { get; set; }
        public int ToUserID { get; set; }
        public DateTime Created { get; set; }

    }
}
