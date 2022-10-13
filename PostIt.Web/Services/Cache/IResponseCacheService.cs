namespace PostIt.Web.Services.Cache;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string key, object response, TimeSpan timeToLive);
    Task<string> GetResponseAsync(string key);
}