using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace Shamrock_WebSite.Helpers
{
    public static class PagingHelper
    {
        public static HtmlString PageLinks(this HtmlHelper helper, int currentPage, int totalPages, Func<int, string> pageUrl)
        {
            var builder = new StringBuilder();
            var min = Math.Max(currentPage - 2, 1);
            var max = Math.Min(currentPage + 2, totalPages);
            if (min == max)
                return null;
            for (int i = min; i <= max; i++)
            {
                var tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                tag.AddCssClass("pageLink");
                if (i == currentPage)
                    tag.AddCssClass("selected");
                builder.AppendLine(tag.ToString());
            }
            return new HtmlString(builder.ToString());
        }
    }
}