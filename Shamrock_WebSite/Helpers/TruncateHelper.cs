using System;
using System.Web.Mvc;

namespace Shamrock_WebSite.Helpers
{
    public static class TruncateHelper
    {
        public static string Truncate(this HtmlHelper helper, string text, string ellipsis, int max, int min = 0)
        {
            if (min > max)
                throw new ArgumentException("min must not be greater than max.");
            if (min < 0)
                throw new ArgumentOutOfRangeException("min");
            if (max < 0)
                throw new ArgumentOutOfRangeException("max");

            if (text.Length <= max)
                return text;

            var lastPosition = max;
            while (text[lastPosition] != ' ' && lastPosition > min)
                lastPosition--;

            return text.Substring(0, lastPosition) + ellipsis;
        }
    }
}