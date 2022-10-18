using System.Text.RegularExpressions;

namespace PostIt.Web.Helpers;

public static class EmailValidatorHelper
{
    public static bool IsValidEmail(this string email) => 
        Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"); 
}