namespace IdentityServer.Application.Entities;

public class UserIdentity
{
    public string Id { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public DateTime RegisterDate { get; set; }
}