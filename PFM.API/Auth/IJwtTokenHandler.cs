using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace PFM.API.Auth
{
    public interface IJwtTokenHandler
    {
        ClaimsPrincipal ValidateToken(string token, TokenValidationParameters tokenValidationParameters);
        string WriteToken(JwtSecurityToken jwt);
    }
}