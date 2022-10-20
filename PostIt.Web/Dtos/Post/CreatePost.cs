namespace PostIt.Web.Dtos.Post;

public class CreatePost
{
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public string? MediaUrl { get; set; }
}