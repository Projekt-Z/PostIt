using PostIt.Web.Data;
using PostIt.Web.Models;

namespace PostIt.Web.Services.Chat;

public class ChatService : IChatService
{
    private readonly ApplicationContext _context;

    public ChatService(ApplicationContext context)
    {
        _context = context;
    }
    
    public List<Message> GetMessages(Guid senderId, Guid receiverId)
    {
        return _context.Messages.Where(x => x.ReceiverId == receiverId && x.UserId == senderId).ToList();
    }
}