using System;
using System.Diagnostics;

namespace Orchard.Park.Core.Exceptions
{
    [DebuggerStepThrough]
    public class TokenException : ApplicationException
    {
        /// <summary>
        /// 返回错误码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 错误描述信息
        /// </summary>
        public override string Message { get; }

        /// <summary>
        /// 错误详细信息
        /// </summary>
        public override string StackTrace { get; }

        public TokenException(string message) : base(message)
        {
            Code = (int)FriendlyExceptionStatusCodeEnum.Token;
            Message = message;
        }

        public TokenException(string message, string stackTrace) : base(message)
        {
            Code = (int)FriendlyExceptionStatusCodeEnum.Token;
            Message = message;
            StackTrace = stackTrace;
        }

        public TokenException(int code, string message) : base(message)
        {
            Code = code;
            Message = message;
        }

        public TokenException(int code, string message, string stackTrace) : base(message)
        {
            Code = code;
            Message = message;
            StackTrace = stackTrace;
        }
    }
}