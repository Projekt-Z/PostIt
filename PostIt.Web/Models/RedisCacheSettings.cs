namespace PostIt.Web.Models;

public class RedisCacheSettings
{
    public bool Enabled { get; set; }
    public string ConnectionString { get; set; } = null!;
}