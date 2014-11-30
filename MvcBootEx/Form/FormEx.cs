using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace MvcBootEx.Form
{
    public static class FormEx
    {
        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type)
        {
            return new BootExForm<TModel>(bootex, null, null, null, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, object routeValues)
        {
            return new BootExForm<TModel>(bootex, null, null, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, RouteValueDictionary routeValues)
        {
            return new BootExForm<TModel>(bootex, null, null, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, null, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            object routeValues)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            RouteValueDictionary routeValues)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            FormMethod method)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, null, type, method, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            object routeValues, FormMethod method)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, routeValues, type, method, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            RouteValueDictionary routeValues, FormMethod method)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, routeValues, type, method, null);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            FormMethod method, object htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, null, type, method, htmlAttributes);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            FormMethod method, IDictionary<string, object> htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, null, type, method, htmlAttributes);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            object routeValues, FormMethod method, object htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, routeValues, type, method, htmlAttributes);
        }

        public static BootExForm<TModel> BeginForm<TModel>(this BootEx<TModel> bootex, FormType type, string actionName, string controllerName,
            RouteValueDictionary routeValues, FormMethod method, IDictionary<string, object> htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, actionName, controllerName, routeValues, type, method, htmlAttributes);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, object routeValues)
        {
            return new BootExForm<TModel>(bootex, null, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, RouteValueDictionary routeValues)
        {
            return new BootExForm<TModel>(bootex, null, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName)
        {
            return new BootExForm<TModel>(bootex, routeName, null, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName, object routeValues)
        {
            return new BootExForm<TModel>(bootex, routeName, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName,
            RouteValueDictionary routeValues)
        {
            return new BootExForm<TModel>(bootex, routeName, routeValues, type, FormMethod.Post, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName, FormMethod method)
        {
            return new BootExForm<TModel>(bootex, routeName, null, type, method, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName, object routeValues,
            FormMethod method)
        {
            return new BootExForm<TModel>(bootex, routeName, routeValues, type, method, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName,
            RouteValueDictionary routeValues, FormMethod method)
        {
            return new BootExForm<TModel>(bootex, routeName, routeValues, type, method, null);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName, FormMethod method,
            object htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, routeName, null, type, method, htmlAttributes);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName, FormMethod method,
            IDictionary<string, object> htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, routeName, null, type, method, htmlAttributes);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName, object routeValues,
            FormMethod method, object htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, routeName, routeValues, type, method, htmlAttributes);
        }

        public static BootExForm<TModel> BeginRouteForm<TModel>(this BootEx<TModel> bootex, FormType type, string routeName,
            RouteValueDictionary routeValues, FormMethod method, IDictionary<string, object> htmlAttributes)
        {
            return new BootExForm<TModel>(bootex, routeName, routeValues, type, method, htmlAttributes);
        }
    }
}