using WebApp.DTOs.Posts;
using WebApp.Model;

namespace WebApp.DTOs.Users;

public class UserAndCommentResponse: UserResponse
{
    public IEnumerable<PostResponse> Posts { get; set; }
    public string Email { get; set; }
    public static implicit operator UserAndCommentResponse(User user)
    {
        return new UserAndCommentResponse
        {
            UserName = user.UserName ?? string.Empty,
            Id = user.Id,
            Email = user.Email ?? string.Empty,
            ProfilePictureLink = user.ProfilePicture,
            Posts = user.Posts.Select(x => PostResponse.FromPost(x, user.Id))
        };
    }
}