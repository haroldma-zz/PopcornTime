using System.Text.RegularExpressions;

namespace PopcornTime.Web.Extensions
{
    public static class StringExtensions
    {
        public static string ToUnderscoreCase(this string text)
        {
            return Regex.Replace(text, "([A-Z])([A-Z][a-z])|([a-z0-9])([A-Z])", "$1$3_$2$4");
        }
    }
}