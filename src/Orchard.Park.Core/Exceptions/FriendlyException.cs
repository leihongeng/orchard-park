using System;
using System.Diagnostics;

namespace Orchard.Park.Core.Exceptions;

[DebuggerStepThrough]
public class FriendlyException : ApplicationException
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

    public FriendlyException(string message) : base(message)
    {
        Code = (int)FriendlyExceptionStatusCodeEnum.System;
        Message = message;
    }

    public FriendlyException(string message, string stackTrace) : base(message)
    {
        Code = (int)FriendlyExceptionStatusCodeEnum.System;
        Message = message;
        StackTrace = stackTrace;
    }

    public FriendlyException(int code, string message) : base(message)
    {
        Code = code;
        Message = message;
    }

    public FriendlyException(int code, string message, string stackTrace) : base(message)
    {
        Code = code;
        Message = message;
        StackTrace = stackTrace;
    }
}

public enum FriendlyExceptionStatusCodeEnum
{
    /// <summary>
    /// 系统错误
    /// </summary>
    System = 400,

    /// <summary>
    /// Token无效
    /// </summary>
    Token = 401,

    /// <summary>
    /// 参数错误
    /// </summary>
    Params = 403
}