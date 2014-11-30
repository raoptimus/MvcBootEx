using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcBootEx.Html
{
    public static class IconEx
    {
        /// <summary>
        /// Тег i с glyphicon
        /// </summary>
        /// <param name="bootex"></param>
        /// <param name="icons">Названия классов иконок через пробел без префикса</param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString Icon(this BootEx bootex, string icons, object htmlAttributes = null)
        {
            if (icons.IsEmpty())
            {
                throw new ArgumentException(@"Null or empty", "icons");
            }

            var builder = new TagBuilder("i");
            var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            builder.MergeAttributes(attr);

            var iconList = new HashSet<string>();

            foreach (var name in icons.Split(' '))
            {
                if (name.Trim().IsEmpty())
                    continue;

                if (name.StartsWith("glyphicon-"))
                {
                    iconList.Add(name);
                }
                else
                {
                    iconList.Add("glyphicon-" + name);
                }
            }

            builder.AddCssClass(String.Join(" ", iconList));

            return new MvcHtmlString(builder.ToString());
        }
    }
}