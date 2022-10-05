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

    public string PasswordHash { get; set; } = default!;
    public string Salt { get; set; } = default!;
}