namespace WebApp.DTOs.Posts;

public class CreatePostRequest
{
    public CreatePostRequest()
    {
    }

    public CreatePostRequest(string authorId, string? content)
    {
        AuthorId = authorId;
        Content = content;
    }

    public string AuthorId { get; set; }
    public string? Content { get; set; }
    public string? ImageLink { get; set; }
    public string? Caption { get; set; } = string.Empty;
}