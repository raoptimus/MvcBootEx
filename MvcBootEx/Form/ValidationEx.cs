using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcBootEx.Form
{
    public static class ValidationEx
    {
        public static void ValidationSummary<TModel>(this BootExForm<TModel> form,
            bool excludePropertyErrors = false, string message = null, object htmlAttributes = null)
        {
            var attr = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            HtmlUtil.AddCssClass(attr, "alert alert-dismissible alert-danger");
            var valid = form.BootEx.Html.ValidationSummary(excludePropertyErrors, message, attr);

            form.BootEx.Html.ViewContext.Writer.Write(valid);
        }
    }
}