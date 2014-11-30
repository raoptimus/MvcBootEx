using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcBootEx.Form
{
    public class BootExForm<TModel> : IDisposable
    {
        public FormType FormType { get; private set; }
        private MvcForm Form { get; set; }
        internal BootEx<TModel> BootEx { get; set; }

        public BootExForm(BootEx<TModel> bootEx, string actionName, string controllerName, object routeValues,
            FormType type, FormMethod method, object htmlAttributes)
        {
            FormType = type;
            BootEx = bootEx;

            Form = bootEx.Html.BeginForm(actionName, controllerName,
                HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues),
                method, MergeCss(htmlAttributes));
        }

        public BootExForm(BootEx<TModel> bootEx, string routeName, object routeValues, FormType type, FormMethod method,
            object htmlAttributes)
        {
            FormType = type;
            BootEx = bootEx;

            Form = bootEx.Html.BeginRouteForm(routeName,
                HtmlHelper.AnonymousObjectToHtmlAttributes(routeValues),
                method,
                MergeCss(htmlAttributes));
        }

        private IDictionary<string, object> MergeCss(object htmlAttributes)
        {
            string cssClass = "";

            switch (FormType)
            {
                case FormType.Horizontal:
                    cssClass = "form-horizontal";
                    break;
                case FormType.Inline:
                    cssClass = "form-inline";
                    break;
                case FormType.Search:
                    cssClass = "form-search";
                    break;
                default:
                    /*case FormType.Vertical:*/
                    cssClass = "form-vertical";
                    break;
            }
           
            var attr = htmlAttributes as IDictionary<string, object> ??
                       HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            if (!attr.ContainsKey("class"))
            {
                attr.Add("class", cssClass);
            }
            else
            {
                attr["class"] += " " + cssClass;
            }

            attr.Add("role", "form");

            return attr;
        }

       

        public void Dispose()
        {
            Form.Dispose();
        }
    }
}