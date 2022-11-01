namespace PostIt.Web.Helpers;

public static class GenerateRandomContent
{
    private static readonly List<string> Contents = new()
    {
        "What a nice weather...",
        "I have seen a cute doggo",
        "I woke up early",
        "This app is amazing",
        "Minecraft!",
        "Wash your hands!",
        "Soap and water!",
        "Support local businesses!",
        "Stay home and play games!",
        "Stay safe!",
        "Nooooooooooooo!",
        "Flower forest TM perfume",
        "Hat Fridays!"
    };
    
    public static string Generate()
    {
        return Contents[Random.Shared.Next(0, Contents.Count)];
    }
}