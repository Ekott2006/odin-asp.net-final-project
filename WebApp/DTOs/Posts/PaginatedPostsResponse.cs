namespace WebApp.DTOs.Posts;

public class PaginatedPostsResponse
{
    public int Pages { get; set; }
    public int PerPage { get; set; }
    public int Total { get; set; }

    public List<PostResponse> Posts { get; set; }
    public int Page { get; set; }
}