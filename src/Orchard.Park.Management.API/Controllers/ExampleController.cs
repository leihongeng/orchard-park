using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orchard.Park.Core.Exceptions;
using Orchard.Park.Identity;
using System.Security.Claims;
using Orchard.Park.Model.Configuration;

namespace Orchard.Park.Management.API.Controllers;

/// <summary>
/// 示例
/// </summary>
public class ExampleController : BaseController
{
    private readonly IJwtService _jwtService;
    private readonly ICurrentUser _currentUser;

    public ExampleController(IJwtService jwtService, ICurrentUser currentUser)
    {
        _jwtService = jwtService;
        _currentUser = currentUser;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<string> GetToken()
    {
        var userId = IdHelper.CreateLongId();//用户Id
        var claims = new List<Claim>
        {
            new(ICurrentUserExt.WxOpenId, "ovnuT5bIQCIPh4qyu0yaV4lsBfpo")//OpenId
        };
        var token = await _jwtService.CreateTokenAsync(userId.ToString(), JwtTokenRoles.OrchardParkManagementAPI, claims);
        return token;
    }

    [HttpGet]
    public IActionResult Test()
    {
        //获取当前登录用户
        var userId = _currentUser.GetUserId();

        //生成一个雪花Id
        var id = IdHelper.CreateLongId();

        throw new FriendlyException("这是一个异常");
        return Ok(id);
    }
}