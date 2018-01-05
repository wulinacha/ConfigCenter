using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigCenter.Dto
{
    public class ResultEnum
    {
        public enum ErrorType
        {
            未登录,
            程序错误,
            无异常
        }
        public enum IsSuccess
        {
            失败,
            成功
        }
    }

}
