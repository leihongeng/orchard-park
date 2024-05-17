using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Orchard.Park.Core;
using Orchard.Park.Core.Exceptions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Orchard.Park.AspNetCore.Middlewares;

/// <summary>
/// 全局异常处理
/// </summary>
[DebuggerStepThrough]
public class ExceptionHandlerMiddleware(
    RequestDelegate _next,
    ILogger<ExceptionHandlerMiddleware> _logger,
    GlobalExceptionCustomFuncModel _customFunc)
{
    public async Task Invoke(HttpContext context)
    {
        FriendlyException exception = null;
        var url = context.Request.Path;
        var customLog = _customFunc.GetCustomLogFunc?.Invoke(context);

        try
        {
            await _next(context);

            if (context.Response.StatusCode == 204) context.Response.StatusCode = 200;
        }
        catch (AggregateException ex)
        {
            foreach (var item in ex.InnerExceptions)
            {
                if (item is FriendlyException customException)
                    exception = customException;
                else
                    exception = new FriendlyException((int)FriendlyExceptionStatusCodeEnum.System, item.Message, ex.StackTrace);
                break;
            }
            _logger.LogError($"[url={url}；Error={ex.Message}]{customLog}");
        }
        catch (FriendlyException ex)
        {
            exception = ex;
            _logger.LogError(!string.IsNullOrWhiteSpace(ex.StackTrace)
                ? $"[url={url}；Error={ex.Message} {ex.StackTrace}]{customLog}"
                : $"[url={url}；Error={ex.Message}]{customLog}");
        }
        catch (TokenException ex)
        {
            _logger.LogError($"[url={url}；Error={ex.Message}]{customLog}");
            //直接返回401
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
        catch (Exception ex)
        {
            exception = new FriendlyException((int)FriendlyExceptionStatusCodeEnum.System, ex.Message, ex.StackTrace);
            var innermostException = GetInnermostException(ex);
            var typeName = innermostException is FriendlyException ? null : $"[{innermostException.GetType().Name}]";
            _logger.LogError($"{typeName}[url={url}；Error={innermostException.Message} {innermostException.StackTrace}]{customLog}");
        }
        finally
        {
            if (exception != null)
            {
                try
                {
                    if (_customFunc.GetCustomReturnValueFunc == null)
                    {
                        var response = new ApiResult
                        {
                            Code = exception.Code,
                            IsSuccess = false,
                            Message = exception.Message
                        };

                        if (!_customFunc.UseDefaultData) response.Data = null;

                        var newtonsoftJsonOptions = context.RequestServices.GetService<IOptions<MvcNewtonsoftJsonOptions>>();
                        if (newtonsoftJsonOptions == null)
                        {
                            //如果没有配置则给一个默认值
                            var serializerSettings = new JsonSerializerSettings
                            {
                                ContractResolver = new CamelCasePropertyNamesContractResolver() //小驼峰
                            };
                            await HandleExceptionAsync(context, JsonConvert.SerializeObject(response, Formatting.Indented, serializerSettings));
                        }
                        else
                        {
                            await HandleExceptionAsync(context, JsonConvert.SerializeObject(response, Formatting.Indented, newtonsoftJsonOptions.Value.SerializerSettings));
                        }
                    }
                    else
                    {
                        var customReturnValue = _customFunc.GetCustomReturnValueFunc.Invoke(context, exception.Code, exception.Message);
                        await HandleExceptionAsync(context, customReturnValue);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError($"系统异常：{e.Message}");
                    await HandleExceptionAsync(context, $"系统异常：{e.Message}");
                }
            }
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, string message)
    {
        if (!context.Response.HasStarted)
            context.Response.ContentType = "application/json;charset=utf-8";

        return context.Response.WriteAsync(message);
    }

    private static Exception GetInnermostException(Exception ex)
    {
        var innermostException = ex;

        while (innermostException.InnerException != null)
            innermostException = innermostException.InnerException;

        return innermostException;
    }
}

public static class ExceptionHandlerMiddlewareExtensions
{
    /// <summary>
    /// 全局异常处理中间件
    /// </summary>
    /// <returns></returns>
    public static IApplicationBuilder UseExceptionHandlerMiddleWare(this IApplicationBuilder builder,
        bool useDefaultData = true,
        Func<HttpContext, string> getCustomLogFunc = null,
        Func<HttpContext, int, string, string> getCustomReturnValueFunc = null)
    {
        var cFunc = new GlobalExceptionCustomFuncModel
        {
            UseDefaultData = useDefaultData,
            GetCustomLogFunc = getCustomLogFunc,
            GetCustomReturnValueFunc = getCustomReturnValueFunc
        };

        return builder.UseMiddleware<ExceptionHandlerMiddleware>(cFunc);
    }
}

public class GlobalExceptionCustomFuncModel
{
    public bool UseDefaultData { get; set; }

    /// <summary>
    /// 获取自定义日志内容
    /// </summary>
    public Func<HttpContext, string> GetCustomLogFunc { get; set; }

    /// <summary>
    /// 获取自定义全局返回值
    /// </summary>
    public Func<HttpContext, int, string, string> GetCustomReturnValueFunc { get; set; }
}