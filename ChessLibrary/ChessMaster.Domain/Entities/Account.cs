using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace ChessMaster.Domain.Entities;

public sealed class Account
{
    public Guid UserId { get; private init; }
    public string Email { get; private set; }
    //public string? PasswordResetToken { get; private set; }
    //public DateTime? PasswordResetTokenExpirationDate { get; private set; }
    public DateTime CreatedDate { get; private init; }
    
    public string Salt { get; private set; }
    public string PasswordHash { get; private set; }

    public Account(
        Guid userId,
        string email,
        //string? passwordResetToken, DateTime? passwordResetTokenExpirationDate, 
        DateTime createdDate, 
        string salt, string passwordHash)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(userId));
        }
        ThrowIfEmailIsNotValid(email);
        
        UserId = userId;
        Email = email;
        
        //PasswordResetToken = passwordResetToken;
        //PasswordResetTokenExpirationDate = passwordResetTokenExpirationDate;
        
        CreatedDate = createdDate;
        
        Salt = salt;
        PasswordHash = passwordHash;
    }
    
    public static Account Create(User user, string email, string password)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }
        
        email = email.Trim();
        string formattedEmail = email.ToLowerInvariant();
        
        byte[] salt, passwordHash;
        using (var hmac = new HMACSHA512())
        {
            salt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
        
        string saltString = Convert.ToBase64String(salt);
        string passwordHashString = Convert.ToBase64String(passwordHash);

        return new Account(
            user.Id,
            formattedEmail,
            DateTime.UtcNow, 
            saltString, passwordHashString
        );
    }

    public bool VerifyByPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(password));
        }

        byte[] saltBytes = Convert.FromBase64String(Salt);
        byte[] passwordHashBytes = Convert.FromBase64String(PasswordHash);
        
        using (var hmac = new HMACSHA512(saltBytes))
        {
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var a = hmac.ComputeHash(passwordHash);

            var passwordHashArray = passwordHashBytes;
            var b = hmac.ComputeHash(passwordHashArray);
            return Xor(a, b) && Xor(passwordHash, passwordHashArray);
        }
    }

    private static void ThrowIfEmailIsNotValid(string email)
    {
        if (!email.Contains('@', StringComparison.InvariantCultureIgnoreCase)
            || email[^1] == '@'
            || email[0] == '@')
        {
            throw new InvalidOperationException("Invalid email.");
        }
    }

    private static bool Xor(byte[] a, byte[] b)
    {
        var x = a.Length ^ b.Length;

        for (var i = 0; i < a.Length && i < b.Length; ++i)
        {
            x |= a[i] ^ b[i];
        }

        return x == 0;
    }

    /*
    [MemberNotNull(nameof(PasswordResetToken))]
    [MemberNotNull(nameof(PasswordResetTokenExpirationDate))]
    public void PasswordResetRequest()
    {
        var now = DateTime.UtcNow;
        if (PasswordResetToken is null || PasswordResetTokenExpirationDate < now)
        {
            PasswordResetToken = $"{Guid.NewGuid()}|{Email}";
        }

        PasswordResetTokenExpirationDate = now.AddDays(1);
    }

    public void PasswordReset(string resetPasswordKey, string newPassword)
    {
        if (string.IsNullOrEmpty(newPassword))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(newPassword));
        }

        if (resetPasswordKey != PasswordResetToken || DateTime.UtcNow >= PasswordResetTokenExpirationDate)
        {
            throw new ArgumentException("Invalid reset password key.");
        }

        SetPassword(newPassword);
        PasswordResetToken = null;
        PasswordResetTokenExpirationDate = null;
    }

    public void ChangePassword(string currentPassword, string newPassword)
    {
        if (string.IsNullOrEmpty(newPassword))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(newPassword));
        }

        if (VerifyByPassword(currentPassword))
        {
            throw new AccountException(ErrorCodes.PasswordNotCorrect);
        }

        SetPassword(newPassword);
    }

    [MemberNotNull(nameof(Salt))]
    [MemberNotNull(nameof(PasswordHash))]
    private void SetPassword(string newPassword)
    {
        using (var hmac = new HMACSHA512())
        {
            Salt = hmac.Key;
            PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newPassword));
        }
    }
    */
}
