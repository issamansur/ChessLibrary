namespace ChessMaster.Domain.Entities;

// Class for user's data
public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    
    public User(Guid id, string username)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id cannot be empty", nameof(id));
        }
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty", nameof(username));
        }
        
        Id = id;
        Username = username;
    }
    
    public static User Create(string username)
    {
        Guid id = Guid.NewGuid();
        username = username.Trim();
        
        return new User(id, username);
    }   
}