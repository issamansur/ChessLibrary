

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ChessMaster.Infrastructure.Services;

public class JwtService: IJwtService
{
    public string GenerateToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.UserId.ToString()),
            //new Claim(ClaimTypes.Name, account.UserName),
        };

        var signingKey = JwtSettings.GetSymmetricSecurityKey();
        
        var jwtToken = new JwtSecurityToken
        (
            issuer: JwtSettings.ISSUER,
            audience: JwtSettings.AUDIENCE,
            claims: claims,
            expires: DateTime.Now.AddMinutes(JwtSettings.EXPIRY_MINUTES),
            signingCredentials: new SigningCredentials
            (
                key: signingKey, 
                algorithm: SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}