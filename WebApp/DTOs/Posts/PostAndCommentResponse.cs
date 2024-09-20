using WebApp.DTOs.Comments;
using WebApp.Model;

namespace WebApp.DTOs.Posts;

public class PostAndCommentResponse: PostResponse
{
    public IEnumerable<CommentResponse> Comments { get; set; }
    
    public new static PostAndCommentResponse FromPost(Post post, string userId)
    {
        return new PostAndCommentResponse
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
            DidUserLike = post.Likes.Any(x => x.Id == userId),
            Comments = post.Comments.Select(x => (CommentResponse)x)
        };
    }
}
