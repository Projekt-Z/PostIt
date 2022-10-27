using System.Text.RegularExpressions;

namespace PostIt.Web.Helpers
{
    public static class PhoneNumberValidatorHelper
    {
        public static bool IsValidPhoneNumber(this string number)
        {
            if (string.IsNullOrEmpty(number))
                return false;

            var regex = new Regex(@"^\(?([0-9]{3})\)?[-.●]?([0-9]{3})[-.●]?([0-9]{3})$");
            return regex.IsMatch(number);
        }
    }
}
