using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace PostIt.Web.Services.Notification;

public class NotificationService : INotificationService
{
    private readonly IDistributedCache _cache;

    public NotificationService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public void Push(Guid userId, Models.Notification notification)
    {
        var cached = _cache.GetString(userId.ToString());

        var notifications = cached is null ? new List<Models.Notification>() : JsonConvert.DeserializeObject<List<Models.Notification>>(cached);

        notifications!.Add(notification);

        var notificationsJson = JsonConvert.SerializeObject(notifications);
        
        _cache.SetString(userId.ToString(), notificationsJson);
    }

    public List<Models.Notification>? Pop(Guid id)
    {
        var cacheJson = _cache.GetString(id.ToString());

        return cacheJson is null ? null : JsonConvert.DeserializeObject<List<Models.Notification>>(cacheJson)?.OrderByDescending(x => x.Time).ToList();
    }
}