namespace PostIt.Web.Helpers;

public static class GenerateRandomTitle
{
    private static readonly List<string> Titles = new()
    {
        "What a nice weather...",
        "I have seen a cute doggo",
        "I woke up early",
        "This app is amazing",
        "Minecraft!"
    };
    
    public static string Generate()
    {
        return Titles[Random.Shared.Next(0, Titles.Count)];
    }
}