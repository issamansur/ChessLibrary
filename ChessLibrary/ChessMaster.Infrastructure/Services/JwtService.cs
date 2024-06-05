using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChessMaster.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ChessMaster.Infrastructure.Services;

public class JwtService: IAuthService
{
    private readonly JwtOptions _options;
    
    public JwtService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public string GenerateToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(CustomClaimTypes.UserId, account.UserId.ToString())
        };
        
        var jwtToken = new JwtSecurityToken
        (
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_options.ExpireHours),
            signingCredentials: new SigningCredentials
            (
                key: new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Key)), 
                algorithm: SecurityAlgorithms.HmacSha256
            )
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}

public static class CustomClaimTypes
{
    public const string UserId = "UserId";
    public const string UserName = "UserName";
}