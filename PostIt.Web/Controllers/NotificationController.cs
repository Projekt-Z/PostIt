using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public NotificationController(INotificationService notificationService, ApplicationContext context)
    {
        _notificationService = notificationService;
        _context = context;
    }
    
    public IActionResult Index()
    {
        var user = _context.Users.FirstOrDefault(x => x.Username == User.Identity!.Name);

        var notification = _notificationService.Pop(user!.Id);

        if (notification is null)
        {
            return View(null);
        }
        
        return View(new List<Notification>
        {
            notification
        });
    }
}