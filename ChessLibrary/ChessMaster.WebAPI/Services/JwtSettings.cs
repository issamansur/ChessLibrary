using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ChessMaster.WebAPI.Services;

public class JwtSettings
{
    public const string ISSUER = "ChessMaster";
    public const string AUDIENCE = "ChessMasterClient";
    
    public const int EXPIRY_MINUTES = 60;

    // 32+ characters
    private const string KEY = "e4e5Kf3Kc6d4!edKd4Cc5Ce3Kf6?Kc6!";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
}