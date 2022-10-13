using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace PostIt.Web.Services.Cache;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;

    public ResponseCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
    {
        var serializedResponse = JsonConvert.SerializeObject(response);

        await _distributedCache.SetStringAsync(key, serializedResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timeToLive
        });

    }

    public async Task<string> GetResponseAsync(string key)
    {
        var cached = await _distributedCache.GetStringAsync(key);

        return string.IsNullOrEmpty(cached) ? null : cached;
    }
}