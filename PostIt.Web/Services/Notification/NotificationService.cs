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
        var json = JsonConvert.SerializeObject(notification);
        
        _cache.SetString(userId.ToString(), json);
    }

    public Models.Notification? Pop(Guid id)
    {
        var cacheJson = _cache.GetString(id.ToString());

        return cacheJson is null ? null : JsonConvert.DeserializeObject<Models.Notification>(cacheJson);
    }
}