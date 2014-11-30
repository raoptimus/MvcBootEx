using System;
using System.Web.Mvc;

namespace MvcBootEx.Html
{
    public enum AlertType
    {
        Success,
        Info,
        Warning,
        Danger
    }
    public static class AlertEx
    {
        public static MvcHtmlString Alert(this BootEx bootex, string innerHtml, AlertType type, object htmlAttributes = null)
        {
            var builder = new TagBuilder("div");
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            builder.AddCssClass("alert alert-dismissible alert-" + Enum.GetName(typeof(AlertType), type).ToLower());
            builder.InnerHtml = "<button type=\"button\" class=\"close\" data-dismiss=\"alert\"> " +
                                "<span aria-hidden=\"true\">&times;</span>" +
                                "<span class=\"sr-only\">Close</span> " +
                                "</button>" +
                                innerHtml;

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}