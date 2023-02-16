using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Enum
{
    public class EnumGender
    {
        public const int MALE = 1;
        public const int FEMALE = 0;
        public const int OTHERS = 2;
        public static string ToString(int? value)
        {
            switch (value)
            {
                case MALE:
                    return "NAM";
                case FEMALE:
                    return "NỮ";
                case OTHERS:
                    return "KHÁC";
                default:
                    return "";
            }
        }
    }
}
