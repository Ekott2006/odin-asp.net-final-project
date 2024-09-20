using WebApp.Model.Enum;

namespace WebApp.Model;

public class PostDetail
{
    public Guid Id { get; set; }
    public required PostType Type { get; set; }
    public string? Content { get; set; }
    public string? Link { get; set; }
    public string Caption { get; set; }
}