using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Enum
{
    public class EnumCustomer
    {
        public const int ADVISE = 1;
        public const int REGISTER = 0;
        public static string ToString(int? value)
        {
            switch (value)
            {
                case ADVISE:
                    return "Cần tư vấn";
                case REGISTER:
                    return "Nhận báo giá";
                default:
                    return "";
            }
        }
    }
}
