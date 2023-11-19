using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Model;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string UserName { get; private set; }
    public string Email { get; private set; }
    public string Password { get; set; }
    public string IdentityUserId { get; init; }

    public User(string userName, string email, string password, string identityUserId)
    {
        UserName = userName;
        Email = email;
        Password = password;
        IdentityUserId = identityUserId;
    }
    
    public User()
    {
        
    }
}