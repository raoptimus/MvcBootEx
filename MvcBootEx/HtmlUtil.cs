using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcBootEx
{
    internal static class HtmlUtil
    {
        public static IDictionary<string, object> ObjectToHtmlAttributesDictionary(object htmlAttributes)
        {
            return htmlAttributes as IDictionary<string, object> ??
                   HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
        }

        public static IDictionary<string, object> AddCssClass(object htmlAttributes, string cssClass)
        {
            var attr = ObjectToHtmlAttributesDictionary(htmlAttributes);

            if (attr.ContainsKey("class"))
            {
                attr["class"] += " " + cssClass;
            }
            else
            {
                attr.Add("class", cssClass);
            }
            
            return attr;
        }
    }
}