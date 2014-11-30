using System;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcBootEx.Html
{
    public static class ButtonEx
    {
        public static MvcHtmlString Button(this BootEx bootEx, string text, Color color = Color.Default,
            string icons = null, object htmlAttributes = null)
        {
            var tag = ButtonHelper(bootEx, "button", text, color, icons, htmlAttributes);

            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString ActiveButton(this BootEx bootEx,
            string actionName, string controllerName, object routeValues, string text, Color color = Color.Default,
            string icons = null, object htmlAttributes = null)
        {
            var tag = ButtonHelper(bootEx, "a", text, color, icons, htmlAttributes);
            var urlHelper = new UrlHelper(bootEx.Html.ViewContext.RequestContext);
            tag.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));

            return new MvcHtmlString(tag.ToString());
        }

        private static TagBuilder ButtonHelper(BootEx bootEx, string tagName, string text, Color color, string icons,
            object htmlAttributes)
        {
            var tag = new TagBuilder(tagName);
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            if (icons.IsEmpty())
            {
                tag.SetInnerText(text);
            }
            else
            {
                tag.InnerHtml = bootEx.Icon(icons) + " " + HttpUtility.HtmlEncode(text);
            }

            tag.AddCssClass("btn btn-" + Enum.GetName(typeof (Color), color).ToLower());
            return tag;
        }
    }
}