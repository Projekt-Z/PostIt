using Microsoft.AspNetCore.SignalR;
using PostIt.Web.Data;
using PostIt.Web.Models;
using PostIt.Web.Services;

namespace PostIt.Web.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly ApplicationContext _context;
    private readonly IUserService _userService;

    public ChatHub(ILogger<ChatHub> logger, ApplicationContext context, IUserService userService)
    {
        _logger = logger;
        _context = context;
        _userService = userService;
    }
    
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

    public async Task SendToUser(string username, string receiverConnectionId, string message, string receiverUsername)
    {
        var user = _userService.GetByUsername(username);
        var receiver = _userService.GetByUsername(receiverUsername);
        
        var msg = new Message
        {
            User = user!,
            UserId = user!.Id,
            ReceiverId = receiver!.Id,
            Text = message,
            DateTime = DateTime.Now.ToString()
        };

        user.Messages ??= new List<Message>();
        
        user.Messages.Add(msg);

        await _context.SaveChangesAsync();
        
        await Clients.Client(receiverConnectionId).SendAsync("ReceiveMessage", username, message);
    }

    // TODO: Override ConnectionId with UserId
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