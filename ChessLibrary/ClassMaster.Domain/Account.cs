using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.ComponentModel;
using ClassMaster.Domain.Exceptions;
using ClassMaster.Domain.Requests;

namespace ClassMaster.Domain;


public sealed class Account : EntityWithEvents
{
    public Account(string email, string password, License? license)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(email));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(password));
        }

        Id = Guid.NewGuid();
        SetEmail(email);
        SetPassword(password);
        License = license;

        CreatedDate = UpdatedDate = DateTime.UtcNow;
        RaiseEvent(new CreatedEvent(Id, DateTime.UtcNow));
    }

    // ReSharper disable once UnusedMember.Local
    private Account()
    {
    }

    public Guid Id { get; private init; }
    public string? Email { get; private set; }
    public string? NormalizedEmail { get; private set; }
    public string? PasswordResetToken { get; private set; }
    public DateTime? PasswordResetTokenExpirationDate { get; private set; }
    public DateTime CreatedDate { get; private init; }
    public DateTime UpdatedDate { get; private set; }
    public IReadOnlyCollection<byte>? Salt { get; private set; }
    public IReadOnlyCollection<byte>? PasswordHash { get; private set; }
    public License? License { get; private set; }

    [MemberNotNullWhen(true, nameof(NormalizedEmail))]
    [MemberNotNullWhen(true, nameof(Salt))]
    [MemberNotNullWhen(true, nameof(PasswordHash))]
    public bool IsEmailAuthorization()
    {
        return NormalizedEmail is not null && Salt is not null && PasswordHash is not null;
    }

    [MemberNotNull(nameof(NormalizedEmail))]
    [MemberNotNull(nameof(Salt))]
    [MemberNotNull(nameof(PasswordHash))]
    public void AddEmailAuthorization(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(email));
        }

        if (!string.IsNullOrEmpty(NormalizedEmail) && NormalizedEmail != email.ToUpperInvariant())
        {
            throw new ArgumentException("Email does not match.", nameof(email));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(password));
        }

        ThrowIfNotEmailAuthorization();

        if (NormalizedEmail is null)
        {
            SetEmail(email);
        }

        SetPassword(password);
        SetUpdatedDate();
    }

    public bool VerifyByPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(password));
        }

        ThrowIfNotEmailAuthorization();

        using (var hmac = new HMACSHA512((byte[])Salt))
        {
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            var a = hmac.ComputeHash(passwordHash);

            var passwordHashArray = (byte[])PasswordHash;
            var b = hmac.ComputeHash(passwordHashArray);
            return Xor(a, b) && Xor(passwordHash, passwordHashArray);
        }
    }

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

        RaiseEvent(new PasswordResetRequestEvent(Id, now));
        SetUpdatedDate();
    }

    public void PasswordReset(string resetPasswordKey, string newPassword)
    {
        if (string.IsNullOrEmpty(newPassword))
        {
            throw new ArgumentException("Value cannot be null or empty.", nameof(newPassword));
        }

        ThrowIfNotEmailAuthorization();

        if (resetPasswordKey != PasswordResetToken || DateTime.UtcNow >= PasswordResetTokenExpirationDate)
        {
            throw new ArgumentException("Invalid reset password key.");
        }

        SetPassword(newPassword);
        PasswordResetToken = null;
        PasswordResetTokenExpirationDate = null;
        SetUpdatedDate();
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
        SetUpdatedDate();
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

    [MemberNotNull(nameof(NormalizedEmail))]
    private void SetEmail(string email)
    {
        ThrowIfEmailIsNotValid(email);

        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
    }

    [MemberNotNull(nameof(NormalizedEmail))]
    [MemberNotNull(nameof(Salt))]
    [MemberNotNull(nameof(PasswordHash))]
    private void ThrowIfNotEmailAuthorization()
    {
        if (!IsEmailAuthorization())
        {
            throw new InvalidOperationException("Not supported email authorization.");
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

    private void SetUpdatedDate()
    {
        UpdatedDate = DateTime.UtcNow;
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
}
