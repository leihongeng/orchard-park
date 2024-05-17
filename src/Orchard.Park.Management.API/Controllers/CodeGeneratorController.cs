using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orchard.Park.Core.Exceptions;
using Orchard.Park.IService.CodeGenerator;
using System.ComponentModel;

namespace Orchard.Park.Management.API.Controllers;

/// <summary>
/// 生成数据库表实体
/// </summary>
[ApiController]
[AllowAnonymous]
public class CodeGeneratorController(ICodeGeneratorService codeGeneratorService) : BaseController
{
    /// <summary>
    /// 生成代码
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Description("生成代码")]
    public IActionResult CodeGenDown()
    {
        const string tableName = "AllTable";
        const string fileType = "AllFiles";

        var data = codeGeneratorService.CodeGenByAll(fileType);
        if (data != null)
        {
            return File(data, System.Net.Mime.MediaTypeNames.Application.Zip, tableName + "-" + fileType + ".zip");
        }

        throw new FriendlyException("获取数据库字段失败");
    }
}