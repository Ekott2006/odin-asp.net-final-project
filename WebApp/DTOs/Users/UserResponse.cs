using WebApp.Model;

namespace WebApp.DTOs.Users;

public class UserResponse
{
    public string UserName { get; set; }
    public string Id { get; set; }
    public string ProfilePictureLink { get; set; }

    public static UserResponse FromUser(User user)
    {
        return new UserResponse
        {
            UserName = user.UserName ?? string.Empty,
            Id = user.Id,
            ProfilePictureLink = user.ProfilePicture
        };
    }
}