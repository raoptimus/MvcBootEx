using System;
using System.Web.Mvc;

namespace MvcBootEx.Html
{
    public static class BadgeEx
    {
        public static MvcHtmlString Badge(this BootEx bootex, string text, Color color, object htmlAttributes = null)
        {
            var builder = new TagBuilder("span");

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            builder.AddCssClass("badge badge-" + Enum.GetName(typeof(Color), color).ToLower());
            builder.InnerHtml = text;

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}