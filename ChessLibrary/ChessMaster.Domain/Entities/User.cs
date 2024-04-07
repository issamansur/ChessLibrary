namespace ChessMaster.Domain.Entities;

// Class for user's data
public class User
{
    public Guid UserId { get; private set; }
    public string Username { get; private set; }
    
    public User(Guid userId, string username)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(userId));
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty", nameof(username));
        }
        
        UserId = userId;
        Username = username;
    }
    
    public static User Create(string username)
    {
        var id = Guid.NewGuid();
        username = username.Trim();
        
        return new User(id, username);
    }   
}