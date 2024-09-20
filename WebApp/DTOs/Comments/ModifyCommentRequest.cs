using System.ComponentModel.DataAnnotations;

namespace WebApp.DTOs.Comments;

public class ModifyCommentRequest
{
    [Required, Length(2, 200)]
    public string Content { get; set; }
    [Required]
    public string UserId { get; set; }
    [Required]
    public Guid CommentId { get; set; }
}