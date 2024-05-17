using Orchard.Park.Core.Exceptions;

namespace Orchard.Park.Identity;

public static class ICurrentUserExt
{
    public const string WxOpenId = "WxOpenId";

    /// <summary>
    /// 获取微信OpenId
    /// </summary>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    /// <exception cref="TokenException"></exception>
    public static string GetWxOpenId(this ICurrentUser currentUser)
    {
        var claim = currentUser.FindClaim(WxOpenId);
        return claim == null ? throw new TokenException($"[Token无效] {nameof(GetWxOpenId)}") : claim.Value;
    }
}