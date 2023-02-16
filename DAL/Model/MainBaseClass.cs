using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class MainBaseClass:BaseClass
    {
        public int UserCreated { get; set; }
        public int UserUpdated { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
