using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcBootEx.Form
{
    public static class FormGroupEx
    {
        public static FormGroup BeginFormGroup(this BootEx bootex, object htmlAttributes = null)
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("form-group");
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            bootex.Html.ViewContext.Writer.Write(tag.ToString());

            return new FormGroup(bootex.Html.ViewContext);
        }

        public static void EndFormGroup(this BootEx bootex)
        {
            EndFormGroup(bootex.Html.ViewContext);
        }

        internal static void EndFormGroup(ViewContext viewContext)
        {
            viewContext.Writer.Write("</div>");
        }

        public static MvcHtmlString HelpBlock(this BootEx bootex, string text, object htmlAttributes = null)
        {
            var tag = new TagBuilder("span");
            tag.AddCssClass("help-block");
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            tag.SetInnerText(text);
            
            return new MvcHtmlString(tag.ToString());
        }

        public static MvcHtmlString HelpBlockFor<TModel, TProperty>(
            this BootEx<TModel> bootex,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, bootex.Html.ViewData);

            if (metaData.Description.IsEmpty())
            {
                return MvcHtmlString.Empty;
            }

            return HelpBlock(bootex, metaData.Description, htmlAttributes);
        }
    }
}