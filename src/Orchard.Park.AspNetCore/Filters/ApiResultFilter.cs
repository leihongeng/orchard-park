using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Orchard.Park.Core;
using Orchard.Park.Core.Attributes;
using Orchard.Park.Core.Exceptions;
using System.Diagnostics;
using System.Linq;

namespace Orchard.Park.AspNetCore.Filters
{
    /// <summary>
    /// 重写Api返回值
    /// </summary>
    [DebuggerStepThrough]
    public class ApiResultFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var isDefined = false;
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(true)
                    .FirstOrDefault(a => a.GetType() == typeof(IgnoreRewriteAttribute)) != null;
            }

            if (isDefined) return;

            switch (context.Result)
            {
                case null:
                case OkResult _:
                case EmptyResult _:
                    context.Result = new JsonResult(new ApiResult());
                    break;

                case ObjectResult result:
                    {
                        if (result.Value is ApiResult || (result.Value != null && typeof(ApiResult<>).Name == result.Value.GetType().Name))
                        {
                            context.Result = new JsonResult(result.Value);
                        }
                        else
                        {
                            context.Result = new JsonResult(new ApiResult<object>(result.Value));
                        }
                    }
                    break;

                case ContentResult result:
                    context.Result = new JsonResult(new ApiResult<string>(result.Content));
                    break;

                case JsonResult _:
                    break;

                case FileContentResult _:
                    break;

                default:
                    throw new FriendlyException($"未处理ApiResult类型：{context.Result.GetType().Name}");
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //忽略
        }
    }
}