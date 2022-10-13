using System.ComponentModel.DataAnnotations;

namespace PostIt.Web.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }

    [MaxLength(25)]
    public string Username { get; set; } = default!;
    
    [MaxLength(25)]
    public string Name { get; set; } = default!;
    [MaxLength(25)]
    public string Surname { get; set; } = default!;

    public string PasswordHash { get; set; } = default!;
    public string Salt { get; set; } = default!;
    public string Email { get; set; } = default!;
    [MaxLength(15)]
    public string PhoneNumber { get; set; } = default!;

    public string CreatedOn { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;

    public List<Post> Posts { get; set; } = default!;
    public List<Post> LikedPosts { get; set; } = default!;
}