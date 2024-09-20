namespace WebApp.Model;

public class Comment
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public required string UserId { get; set; }
    public User Commenter { get; set; }
    public required Guid PostId { get; set; }
    public Post Post { get; set; }
}