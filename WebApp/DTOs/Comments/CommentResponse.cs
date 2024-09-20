using WebApp.Model;

namespace WebApp.DTOs.Comments;

public class CommentResponse
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public required string UserId { get; set; }
    public required Guid PostId { get; set; }
    public string UserName { get; set; }
    public string ProfilePicture { get; set; } = string.Empty;
    
    public static implicit operator CommentResponse(Comment comment)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            UserId = comment.UserId,
            PostId = comment.PostId,
            UserName = comment.Commenter.UserName ?? string.Empty,
            ProfilePicture = comment.Commenter.ProfilePicture,

        };
    }
}