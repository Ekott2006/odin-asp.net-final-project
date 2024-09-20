using WebApp.Model;
using WebApp.Model.Enum;

namespace WebApp.DTOs.Posts;

public class PostResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public PostType PostType { get; set; }
    public string? ImageLink { get; set; }
    public string? Caption { get; set; }
    public string? Content { get; set; }
    public int LikesCount { get; set; }
    public string ProfilePictureLink { get; set; }
    public int CommentsCount { get; set; }

    public bool DidUserLike { get; set; }
    // TODO: Use Implicit Operator
    public static PostResponse FromPost(Post post, string userId)
    {
        return new PostResponse
        {
            Id = post.Id,
            UserName = post.Author.UserName ?? string.Empty,
            PostType = post.PostDetail.Type,
            ImageLink = post.PostDetail.Link,
            Caption = post.PostDetail.Caption,
            Content = post.PostDetail.Content,
            LikesCount = post.Likes.Count,
            ProfilePictureLink = post.Author.ProfilePicture,
            CommentsCount = post.Comments.Count,
            DidUserLike = post.Likes.Any(x => x.Id == userId)
        };
    }
}