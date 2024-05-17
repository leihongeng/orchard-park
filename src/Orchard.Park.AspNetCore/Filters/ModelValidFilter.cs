using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Orchard.Park.Core.Exceptions;
using System.Diagnostics;
using System.Linq;

namespace Orchard.Park.AspNetCore.Filters
{
    /// <summary>
    /// 模型参数校验
    /// </summary>
    [DebuggerStepThrough]
    public class ModelValidFilter(IHostEnvironment hostEnvironment) : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                foreach (var item in context.ModelState.Keys)
                {
                    var errorItem = context.ModelState[item];

                    if (errorItem == null) continue;
                    if (!errorItem.Errors.Any()) continue;

                    var error = errorItem.Errors.FirstOrDefault();
                    if (error == null) continue;

                    if (error.ErrorMessage == "A non-empty request body is required.")
                        throw new FriendlyException((int)FriendlyExceptionStatusCodeEnum.Params, "缺失必要参数");

                    if (error.ErrorMessage.Contains("converting value {null} to type"))
                        throw new FriendlyException((int)FriendlyExceptionStatusCodeEnum.Params, $"[{item}]参数不能为空");

                    var msg = error.Exception == null
                        ? hostEnvironment.IsDevelopment() ? $"[{item}] {error.ErrorMessage}" : error.ErrorMessage
                        : $"{item}参数异常";

                    throw new FriendlyException((int)FriendlyExceptionStatusCodeEnum.Params, msg);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}