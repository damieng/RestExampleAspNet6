using Microsoft.IdentityModel.Tokens;
using RestApi.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RestApi.Infrastructure
{
    internal static class AuthenticationHelper
    {
        public static IEnumerable<Claim> CreateClaims(Guid userId, string name, string[] roles)
        {
            var claims = new List<Claim>
            {
                new("userId", userId.ToString()),
                new("name", name)
            };
            if (roles.Any())
                claims.AddRange(roles.Select(role => new Claim("roles", role)));
            return claims;
        }

        public static JwtSecurityToken CreateJwtToken(JwtOptions options, IEnumerable<Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.Now.AddMinutes(options.ExpiresMinutes);

            return new JwtSecurityToken(options.Issuer,
              options.Audience,
              claims.ToArray(),
              signingCredentials: signingCredentials,
              expires: expiration);
        }

        public static string SerializeJwtToken(JwtSecurityToken token)
        {
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
