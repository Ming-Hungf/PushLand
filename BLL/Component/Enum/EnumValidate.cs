using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Component.Enum
{
    public class EnumValidate
    {
        public const int VALIDATED = 1;
        public const int PENDING = 0;
        public const int REJECTED = -1;
        public static string ToString(int? value)
        {
            switch (value)
            {
                case VALIDATED:
                    return "Đã phê duyệt";
                case PENDING:
                    return "Chưa xét duyệt";
                case REJECTED:
                    return "Bị từ chối";
                default:
                    return "";
            }
        }
    }
}
