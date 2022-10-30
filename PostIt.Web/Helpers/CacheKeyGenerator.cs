namespace PostIt.Web.Helpers;

public static class CacheKeyGenerator
{
    public static string Generate(string type, Guid userId)
    {
        return $"{userId} - {type}";
    }
}