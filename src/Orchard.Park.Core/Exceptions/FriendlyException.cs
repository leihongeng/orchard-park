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
    /// 答复超时
    /// </summary>
    ReplyTimeout = 40001,

    /// <summary>
    /// 参数错误
    /// </summary>
    Params = 406,

    /// <summary>
    /// 未插枪
    /// </summary>
    NotConnected = 40002,

    /// <summary>
    /// 充电结束
    /// 需要拔枪后再重新启动
    /// </summary>
    ChargingCompleted = 40003,

    /// <summary>
    /// 存在超时占位费
    /// </summary>
    HasIllegalParkingOrder = 40004
}