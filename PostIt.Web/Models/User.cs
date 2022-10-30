using System.ComponentModel.DataAnnotations;
using PostIt.Web.Enums;

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
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public string? BackgroundUrl { get; set; }
    public string? ConnectionId { get; set; }
    public ERoleType Roles { get; set; }
    public EAuthType AccountType { get; set; }
    public bool VerifiedEmail { get; set; }

    public List<Post> Posts { get; set; } = default!;
    public List<Post> PostLiked { get; set; } = default!;
    public List<Followers> Followers { get; set; } = new();
    public List<Following> Following { get; set; } = new();
    public List<Comment>? Replies { get; set; } = new ();
    public List<BlockedUser> BlockedUsers { get; set; } = default!;
    public List<Message>? Messages { get; set; }
}