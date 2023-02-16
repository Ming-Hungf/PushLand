using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Enum
{
    public class EnumEstateType
    {
        public const int FLAT = 1;
        public const int RESIDENT = 0;
        public static string ToString(int? value)
        {
            switch (value)
            {
                case FLAT:
                    return "CHUNG CƯ";
                case RESIDENT:
                    return "THỔ CƯ";
                default:
                    return "";
            }
        }
    }
}
