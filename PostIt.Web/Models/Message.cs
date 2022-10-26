namespace PostIt.Web.Models;

public class Message
{
    public int Id { get; set; }
    
    public Guid UserId { get; set; }
    public Guid ReceiverId { get; set; }
    public virtual User User { get; set; } = default!;
    public string Text { get; set; } = default!;
    public string DateTime { get; set; } = default!;
}