namespace PostIt.Web.Services.Notification;

public interface INotificationService
{
    public void Push(Guid userId, Models.Notification notification);
    public Models.Notification? Pop(Guid id);
}