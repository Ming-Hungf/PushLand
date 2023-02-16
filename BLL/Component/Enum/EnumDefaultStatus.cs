using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Component.Enum
{
    public class EnumDefaultStatus
    {
        public const int ACTIVE = 1;
        public const int DELETED = 0;
        public static string ToString(int? value)
        {
            switch (value)
            {
                case ACTIVE:
                    return "Hoạt động";
                case DELETED:
                    return "Đã xóa";
                default:
                    return "";
            }
        }
    }
}
