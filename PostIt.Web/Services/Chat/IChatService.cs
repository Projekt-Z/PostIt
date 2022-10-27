using PostIt.Web.Models;

namespace PostIt.Web.Services.Chat;

public interface IChatService
{
    public List<Message> GetMessages(Guid senderId, Guid receiverId);
}