using Microsoft.AspNetCore.SignalR;
using PostIt.Web.Data;

namespace PostIt.Web.Hubs;

public class ChatHub : Hub
{
    private readonly ApplicationContext _context;

    public ChatHub(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendToUser(string username, string receiverConnectionId, string message)
    {
        await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", username, message);
    }

    public string GetConnectionId()
    {
        var connectionId = Context.ConnectionId;

        // TODO: Do not store this here => implement redis and store this like (UserId, ConnectionId)
        var user = _context.Users.FirstOrDefault(x => x.Username == Context.User!.Identity!.Name);

        user!.ConnectionId = connectionId;
        _context.SaveChanges();
        
        return connectionId;
    }
}