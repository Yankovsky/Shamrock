using System.Web;
using System.Web.Mvc;

namespace Shamrock_WebSite.Helpers
{
    public static class ScriptHelper
    {
        public static HtmlString Script(this HtmlHelper helper, string scriptName)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var builder = new TagBuilder("script");
            builder.MergeAttribute("src", urlHelper.Content("~/Scripts/" + scriptName));
            builder.MergeAttribute("type", "text/javascript");

            var html = new HtmlString(builder.ToString(TagRenderMode.StartTag) + builder.ToString(TagRenderMode.EndTag));
            return html;
        }
    }
}