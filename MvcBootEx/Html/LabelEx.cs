using System;
using System.Web.Mvc;

namespace MvcBootEx.Html
{
    public static class LabelEx
    {
        public static MvcHtmlString Label(this BootEx bootex, string text, Color color, object htmlAttributes = null)
        {
            var builder = new TagBuilder("span");

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            builder.AddCssClass("label label-" + Enum.GetName(typeof(Color), color).ToLower());
            builder.InnerHtml = text;

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}