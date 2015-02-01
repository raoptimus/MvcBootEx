using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using MvcBootEx.Html;

namespace MvcBootEx.Navigation
{
    public static class LinkEx
    {
        public static MvcHtmlString ActiveLink(
            this BootEx bootex,
            string linkText,
            string actionName,
            object routeValues,
            object htmlAttributes,
            string icons)
        {
            return ActiveLink(bootex, linkText, actionName, null /* controllerName */, routeValues,
                htmlAttributes, icons);
        }

        public static MvcHtmlString ActiveLink(
            this BootEx bootex,
            string linkText,
            string actionName,
            string controllerName,
            object htmlAttributes,
            string icons)
        {
            return ActiveLink(bootex, linkText, actionName, controllerName, null /* routeValues */,
                htmlAttributes, icons);
        }

        public static MvcHtmlString ActiveLink(
            this BootEx bootex,
            string linkText,
            string actionName,
            object htmlAttributes,
            string icons)
        {
            return ActiveLink(bootex, linkText, actionName, null /* controllerName */, null /* routeValues */,
                htmlAttributes, icons);
        }

        /// <summary>
        /// Помечает активную ссылку классом active
        /// </summary>
        /// <param name="bootex">   </param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="htmlAttributes"></param>
        /// <param name="icons"></param>
        /// <returns></returns>
        public static MvcHtmlString ActiveLink(
            this BootEx bootex,
            string linkText,
            string actionName,
            string controllerName,
            object routeValues,
            object htmlAttributes,
            string icons)
        {
            if (linkText.IsEmpty())
            {
                throw new ArgumentException(@"Null or empty", "linkText");
            }

            var urlHelper = new UrlHelper(bootex.Html.ViewContext.RequestContext);
            var linkUrl = urlHelper.Action(actionName, controllerName, routeValues);
            var isCurRoute = IsCurrentRoute(actionName, controllerName, routeValues, true);

            return ActiveLink(bootex, linkText, linkUrl, icons, htmlAttributes, isCurRoute);
        }

        internal static bool IsCurrentRoute(string actionName, string controllerName,
            object routeValues, bool withParams)
        {
            var viewContext = HttpContext.Current.Request.RequestContext;
            var curController = viewContext.RouteData.Values["controller"].ToString();
            var curAction = viewContext.RouteData.Values["action"].ToString();
//            var curArea = viewContext.RouteData.DataTokens["area"];
            var nss = (string[])viewContext.RouteData.DataTokens["namespaces"];

            bool isCa = controllerName == curController && actionName == curAction;

            if (!isCa)
                return false;

            if (!withParams)
                return true;
  
            var routeVals = HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues);
            var query = HttpContext.Current.Request.QueryString;

            if (routeVals.Keys.Any(k => query[k] == null || query[k] != routeVals[k].ToString()))
            {
                return false;
            }

            var className = controllerName + "Controller";
            var types = new List<Type>();

            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    types.AddRange(a.GetTypes().Where(t => t.Name == className));
                }
                catch (ReflectionTypeLoadException)
                {
                }
            }

            if (!types.Any())
                return false;

            Type curControllerType = types.FirstOrDefault(t => nss.Any(ns => t.FullName.StartsWith(ns.Substring(0, ns.Length - 1)))) ?? types.First();

            var mds = curControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(method => method.Name == curAction && !method.IsDefined(typeof(NonActionAttribute)))
                .ToList();

            foreach (var mi in mds)
            {
                int findCount = 0;

                foreach (var pm in mi.GetParameters())
                {
                    if (routeVals.ContainsKey(pm.Name))
                    {
                        findCount++;
                        continue;
                    }

                    if (pm.IsOptional && query[pm.Name] == null)
                    {
                        continue;
                    }

                    findCount = -1;
                    break;
                }

                if (routeVals.Count == findCount)
                {
                    return true;
                }
            }

            return false;
        }

        public static MvcHtmlString ActiveLink(
            this BootEx bootex,
            string linkText,
            string linkUrl,
            string icons = null,
            object htmlAttributes = null,
            bool isActive = false)
        {
            if (linkText.IsEmpty())
            {
                throw new ArgumentException(@"Null or empty", "linkText");
            }

            linkText = HttpUtility.HtmlEncode(linkText);
            var builder = new TagBuilder("a");
            var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            builder.MergeAttributes(attr);

            if (!icons.IsEmpty())
            {
                linkText = bootex.Icon(icons) + " " + linkText;
//                builder.AddCssClass("btn");
            }

            builder.InnerHtml = linkText;
            builder.MergeAttribute("href", linkUrl);

            if (isActive || linkUrl == HttpContext.Current.Request.RawUrl)// || IsActiveUrl(linkUrl))
            {
                builder.AddCssClass("active");
            }

            return new MvcHtmlString(builder.ToString());
        }

        private static readonly string[] _skipGetParams = { "page", "sort", "skip", "sortdir" };

        public static bool IsActiveUrl(string url, bool withGetParams = true)
        {
            string path = HttpContext.Current.Request.Path;
            var getParams = HttpContext.Current.Request.QueryString;
            var acceptParams = "";

            foreach (var getKey in getParams.AllKeys)
            {
                if (_skipGetParams.Any(x => x.Equals(getKey)))
                    continue;

                acceptParams += getKey + "=" + getParams[getKey] + "&";
            }

            if (!acceptParams.IsEmpty())
            {
                acceptParams = "?" + acceptParams.Substring(0, acceptParams.Length - 1);
            }

            return ((path + acceptParams) == url);
        }
    }
}