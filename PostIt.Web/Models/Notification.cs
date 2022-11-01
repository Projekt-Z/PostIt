using PostIt.Web.Enums;

namespace PostIt.Web.Models;

public class Notification
{
    public ENotificationType Type { get; set; }
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
}