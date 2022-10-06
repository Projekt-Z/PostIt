using System.ComponentModel.DataAnnotations;

namespace PostIt.Web.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string TimeAdded { get; set; } = default!;

    public User Author { get; set; } = default!;
}