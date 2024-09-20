namespace WebApp.Model;

public class Post
{
    public Guid Id { get; set; }
    
    public required string AuthorId { get; set; }
    public User Author { get; set; }
    public List<Comment> Comments { get; set; } = [];
    public List<User> Likes { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public required PostDetail PostDetail { get; set; }
}