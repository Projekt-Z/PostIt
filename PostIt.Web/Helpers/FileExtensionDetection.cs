using System.Text.RegularExpressions;

namespace PostIt.Web.Helpers;

public enum EFiles
{
    Image,
    Mp4
}

public static class FileExtensionDetection
{
    public static EFiles? DetectExtension(this string filePath)
    {
        var regex = new Regex(@"^.*\.(jpg|JPG|gif|GIF|doc|DOC|pdf|PDF|mp4|MP4|png|PNG)$");

        if (!regex.IsMatch(filePath)) return null!;
        
        if (filePath.Contains(".jpg") || filePath.Contains(".png") || filePath.Contains(".gif"))
        {
            return EFiles.Image;
        }

        if (filePath.Contains(".mp4"))
        {
            return EFiles.Mp4;
        }

        return null!;
    }
}