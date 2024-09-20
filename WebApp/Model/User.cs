using Microsoft.AspNetCore.Identity;

namespace WebApp.Model;

public class User: IdentityUser
{
    public string ProfilePicture { get; set; } = string.Empty;
    public List<Post> Posts { get; set; } = [];
}