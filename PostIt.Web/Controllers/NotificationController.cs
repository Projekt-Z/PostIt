using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using PostIt.Web.Data;
using PostIt.Web.Models;
using PostIt.Web.Services.Notification;

namespace PostIt.Web.Controllers;

[Authorize]
[Route("[controller]")]
public class NotificationController : Controller
{
    private readonly INotificationService _notificationService;
    private readonly ApplicationContext _context;
    private readonly IDistributedCache _cache;

    public NotificationController(INotificationService notificationService, ApplicationContext context, IDistributedCache cache)
    {
        _notificationService = notificationService;
        _context = context;
        _cache = cache;
    }
    
    public IActionResult Index()
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == User.Identity!.Name);

        var notifications = _notificationService.Pop(user!.Id);

        if (notifications is null)
        {
            return View(null);
        }
        
        return View(notifications);
    }
}