using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Shamrock_WebSite.Infrastructure;

namespace Shamrock_WebSite.Helpers
{
    public static class ImageHelper
    {
        private static string _thumbExtension = "thumb_";
        private static string _thumbFolder = "Thumb";

        public static HtmlString ActionImage(this HtmlHelper helper, string src, string alt, string actionName, string controllerName,
            object routeValues = null, object imgHtmlAttributes = null, object aHtmlAttributes = null)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var url = urlHelper.Action(actionName, controllerName, routeValues);
            return helper.ActionImage(src, alt, url, imgHtmlAttributes, aHtmlAttributes);
        }

        public static HtmlString ActionImage(this HtmlHelper helper, string src, string alt, string url,
            object imgHtmlAttributes = null, object aHtmlAttributes = null)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var aBuilder = new TagBuilder("a");
            aBuilder.MergeAttribute("href", url);
            aBuilder.MergeAttributes(new RouteValueDictionary(aHtmlAttributes));

            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", urlHelper.Content(src));
            imgBuilder.MergeAttribute("alt", alt);
            imgBuilder.MergeAttributes(new RouteValueDictionary(imgHtmlAttributes));

            aBuilder.InnerHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);
            return new HtmlString(aBuilder.ToString());
        }

        public static HtmlString Image(this HtmlHelper helper, string url, string alternateText, object htmlAttributes = null, bool isThumb = false)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var builder = new TagBuilder("img");
            if (isThumb)
                builder.MergeAttribute("src", urlHelper.Thumb(url));
            else
                builder.MergeAttribute("src", urlHelper.Content(url));
            builder.MergeAttribute("alt", alternateText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            return new HtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static string Thumb(this UrlHelper helper, string path)
        {
            var thumbFileName = _thumbExtension + Path.GetFileName(path);
            var thumbFilePath = Path.Combine(PathExtensions.GetDirectoryPath(path), _thumbFolder, thumbFileName);
            return helper.Content(thumbFilePath);
        }

        public static HtmlString RandomImage(this HtmlHelper helper, string directoryPath, string alternateText, object htmlAttributes = null, bool isThumb = false)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            var path = urlHelper.RequestContext.HttpContext.Server.MapPath(directoryPath);
            var files = new DirectoryInfo(path).GetFiles();
            if (files.Count() == 0)
                return null;

            var randomFile = files.Random();
            var imagePath = Path.Combine(directoryPath, randomFile.Name);

            return helper.Image(imagePath, alternateText, htmlAttributes, isThumb);
        }
    }    
}