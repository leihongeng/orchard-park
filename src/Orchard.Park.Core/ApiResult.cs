using Newtonsoft.Json;
using System.ComponentModel;

namespace Orchard.Park.Core;

public class ApiResult<T>
{
    public ApiResult(T data, int code = 0, string msg = "请求成功", StatusCodeType statusCode = StatusCodeType.Success)
    {
        IsSuccess = statusCode == StatusCodeType.Success;
        Code = code;
        Message = msg;
        Data = data;
    }

    [JsonConstructor]
    public ApiResult(int code = 0, string msg = "请求成功", StatusCodeType statusCode = StatusCodeType.Success)
    {
        IsSuccess = statusCode == StatusCodeType.Success;
        Code = code;
        Message = msg;
    }

    /// <summary>
    /// 是否请求成功
    /// </summary>
    [JsonProperty("isSuccess")]
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 返回信息
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; }

    /// <summary>
    /// 0=成功
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// 返回数据
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; }
}

public class ApiResult : ApiResult<object>
{
    [JsonConstructor]
    public ApiResult(int code = 0, string msg = "请求成功", StatusCodeType statusCode = StatusCodeType.Success)
    {
        IsSuccess = statusCode == StatusCodeType.Success;
        Code = code;
        Message = msg;
    }
}

public enum StatusCodeType
{
    /// <summary>
    /// 请求成功
    /// </summary>
    [Description("请求成功")]
    Success = 200,

    /// <summary>
    /// 系统异常
    /// </summary>
    [Description("系统异常")]
    Error = 500,

    /// <summary>
    /// 未授权
    /// </summary>
    [Description("未授权")]
    Unauthorized = 401
}