using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Orchard.Park.Identity
{
    public interface IJwtService
    {
        public Task<string> CreateTokenAsync(string username, string role);

        public Task<string> CreateTokenAsync(string username, string role, List<Claim> claims);
    }

    public class JwtService : IJwtService
    {
        private readonly JwtTokenConfig _config;

        public JwtService(JwtTokenConfig config)
        {
            _config = config;
        }

        public Task<string> CreateTokenAsync(string username, string role)
        {
            // 添加一些必要的键值对
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(ClaimTypes.Role, role)//角色
            };

            var keyBytes = Encoding.UTF8.GetBytes(_config.SecretKey);
            var scs = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config.Issuer,// 签发者
                audience: _config.Audience,// 接收者
                claims: claims,// payload
                expires: DateTime.Now.AddMinutes(_config.ExpireMinutes),// 过期时间
                signingCredentials: scs);// 令牌

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Task.FromResult(token);
        }

        public Task<string> CreateTokenAsync(string username, string role, List<Claim> claims)
        {
            // 添加一些必要的键值对
            claims.Add(new Claim(ClaimTypes.NameIdentifier, username));
            claims.Add(new Claim(ClaimTypes.Role, role));

            var keyBytes = Encoding.UTF8.GetBytes(_config.SecretKey);
            var scs = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config.Issuer,// 签发者
                audience: _config.Audience,// 接收者
                claims: claims,// payload
                expires: DateTime.Now.AddMinutes(_config.ExpireMinutes),// 过期时间
                signingCredentials: scs);// 令牌

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Task.FromResult(token);
        }
    }
}