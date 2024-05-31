namespace ChessMaster.Infrastructure.Options;

public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    
    
    public int ExpiryHours { get; set; }

    // 32+ characters
    public string Key { get; set; }
    
    /*
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    */
}