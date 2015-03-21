using System;
using System.Collections.Generic;
using System.Linq;
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
            var iconSpecList = new HashSet<string>();

            foreach (var name in icons.Split(' ').Where(name => !name.Trim().IsEmpty()))
            {
                if (name.StartsWith("glyphicon-") || name.StartsWith("halfling-"))
                {
                    iconList.Add(name);
                }
                else
                {
                    if (icons.IndexOf(name + "-") != -1)
                    {
                        iconSpecList.Add(name);
                        iconList.Add(name);
                        continue;
                    }

                    if (iconSpecList.Any(spec => name.StartsWith(spec + "-")))
                    {
                        iconList.Add(name);
                        continue;
                    }

                    iconList.Add("glyphicon-" + name);
                }
            }

            builder.AddCssClass(String.Join(" ", iconList));

            return new MvcHtmlString(builder.ToString());
        }
    }
}