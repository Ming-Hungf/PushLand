using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Enum
{
    public class EnumTransactionType
    {
        public const int SALE = 1;
        public const int HIRE = 0;
        public static string ToString(int? value)
        {
            switch (value)
            {
                case SALE:
                    return "BÁN";
                case HIRE:
                    return "CHO THUÊ";
                default:
                    return "";
            }
        }
    }
}
