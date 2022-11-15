namespace PostIt.Web.Helpers;

public static class DateTimeParser
{
    public static string TimeAgo(this DateTime dateTime)
    {
        var timeSpan = DateTime.Now - dateTime;
        
              
        if (timeSpan < TimeSpan.FromSeconds(2))
        {
            return "Now";
        }
        
        if (timeSpan < TimeSpan.FromSeconds(59))
        {
            return $"{timeSpan.Seconds} seconds ago";
        }
        
        if (timeSpan > TimeSpan.FromSeconds(59) && timeSpan < TimeSpan.FromMinutes(2))
        {
            return $"{timeSpan.Minutes} minute ago"; 
        }
        
        if (timeSpan > TimeSpan.FromMinutes(1) && timeSpan < TimeSpan.FromHours(1))
        {
            return $"{timeSpan.Minutes} minutes ago";
        }
        
        if (timeSpan > TimeSpan.FromMinutes(59) && timeSpan < TimeSpan.FromHours(2))
        { 
            return $"{timeSpan.Hours} hour ago";
        }
        
        if(timeSpan > TimeSpan.FromHours(1) && timeSpan < TimeSpan.FromHours(24))
        {
            return $"{timeSpan.Hours} hours ago";
        }

        return dateTime.Date.ToString("d");
    } 
}