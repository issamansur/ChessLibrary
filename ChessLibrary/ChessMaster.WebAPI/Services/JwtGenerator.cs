using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ChessMaster.WebAPI.Services;

public class JwtGenerator
{
    public static string GenerateJwtToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, account.UserId.ToString()),
            //new Claim(ClaimTypes.Name, account.UserName),
        };

        var signingKey = JwtSettings.GetSymmetricSecurityKey();
        var jwtToken = new JwtSecurityToken(
            issuer: JwtSettings.ISSUER,
            audience: JwtSettings.AUDIENCE,
            claims: claims,
            expires: DateTime.Now.AddMinutes(JwtSettings.EXPIRY_MINUTES),
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}