using System;

namespace User.Core.Api
{
    /// <summary>
    /// 用户操作自定义异常类
    /// </summary>
    public class UserOperationException:Exception
    {
        public UserOperationException() { }

        public UserOperationException(string message) : base(message)
        {
        }

        public UserOperationException(string message,Exception innException) : base(message, innException) { }
    }
}