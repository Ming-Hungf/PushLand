using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Model
{
    public class APIResult: ApiResult<object>
    {
    }
    public class ApiResult<T>
    {
        public T Data { get; set; }
        public int Status { get; set; }
        public string Code
        {
            get
            {
                if (Status == 1)
                    return ResultCodes.Success.ToString();
                else return ResultCodes.Fail.ToString();
            }
        }
        public string Message { get; set; }
        public enum ResultCodes
        {
            Success = 1,
            Fail = 0
        }
    }
}
