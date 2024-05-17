using Microsoft.AspNetCore.Http;
using Orchard.Park.Core.Exceptions;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;

namespace Orchard.Park.Identity;

public interface ICurrentUser
{
    long GetUserId();

    string GetUserIdOrDefault();

    Claim FindClaim(string claimType);
}

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    private readonly ClaimsPrincipal _claimsPrincipal = httpContextAccessor.HttpContext?.User ?? Thread.CurrentPrincipal as ClaimsPrincipal;

    public virtual Claim FindClaim(string claimType)
    {
        return _claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
    }

    public long GetUserId()
    {
        var userIdClaim = FindClaim(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            throw new TokenException($"[Token无效] {nameof(GetUserId)}");

        return Convert.ToInt64(userIdClaim.Value);
    }

    public string GetUserIdOrDefault()
    {
        var userIdClaim = FindClaim(ClaimTypes.NameIdentifier);
        return userIdClaim?.Value;
    }
}