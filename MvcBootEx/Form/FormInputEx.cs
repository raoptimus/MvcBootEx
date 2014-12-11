using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MvcBootEx.Form
{
    public static class FormInputEx
    {
        public static void TextBoxRowFor<TModel, TProperty>(this BootExForm<TModel> form,
            Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var group = new TagBuilder("div");
            group.AddCssClass("form-group");
            var htmlAttr = HtmlUtil.AddCssClass(htmlAttributes, "form-control");

            var labelClassCss = "control-label";

            if (form.FormType == FormType.Horizontal)
            {
                labelClassCss += " col-sm-2";
            }

            var label = form.BootEx.Html.LabelFor(expression, new { @class = labelClassCss });
            var input = form.BootEx.Html.TextBoxFor(expression, htmlAttr);

            if (form.FormType == FormType.Horizontal)
            {
                var inputWrapper = new TagBuilder("div");
                inputWrapper.AddCssClass("col-sm-10");
                input = MvcHtmlString.Create(inputWrapper.ToString(TagRenderMode.StartTag) +
                                             input +
                                             inputWrapper.ToString(TagRenderMode.EndTag));
            }

            var helpAttr = (form.FormType == FormType.Horizontal) ? new {@class = "col-sm-offset-2 col-sm-10"} : null;
            var help = form.BootEx.HelpBlockFor(expression, helpAttr);

            group.InnerHtml = label.ToHtmlString() + input + help;
            form.BootEx.Html.ViewContext.Writer.Write(group.ToString());
        }

        public static void TextAreaBoxRowFor<TModel, TProperty>(this BootExForm<TModel> form,
           Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var group = new TagBuilder("div");
            group.AddCssClass("form-group");
            var htmlAttr = HtmlUtil.AddCssClass(htmlAttributes, "form-control");

            var labelClassCss = "control-label";

            if (form.FormType == FormType.Horizontal)
            {
                labelClassCss += " col-sm-2";
            }

            var label = form.BootEx.Html.LabelFor(expression, new { @class = labelClassCss });
            var input = form.BootEx.Html.TextAreaFor(expression, htmlAttr);

            if (form.FormType == FormType.Horizontal)
            {
                var inputWrapper = new TagBuilder("div");
                inputWrapper.AddCssClass("col-sm-10");
                input = MvcHtmlString.Create(inputWrapper.ToString(TagRenderMode.StartTag) +
                                             input +
                                             inputWrapper.ToString(TagRenderMode.EndTag));
            }

            var helpAttr = (form.FormType == FormType.Horizontal) ? new { @class = "col-sm-offset-2 col-sm-10" } : null;
            var help = form.BootEx.HelpBlockFor(expression, helpAttr);

            group.InnerHtml = label.ToHtmlString() + input + help;

            form.BootEx.Html.ViewContext.Writer.Write(group.ToString());
        }

        public static void CheckboxRowFor<TModel, TProperty>(this BootExForm<TModel> form,
            Expression<Func<TModel, TProperty>> expression,
            Expression<Func<TModel, bool>> checkedExpression, 
            object htmlAttributes = null)
        {
            var group = new TagBuilder("div");
            group.AddCssClass("form-group");
            var htmlAttr = HtmlUtil.AddCssClass(htmlAttributes, "form-control");

            var label = form.BootEx.Html.LabelFor(expression);
            var input = form.BootEx.Html.CheckBoxFor(checkedExpression);
            var helpAttr = (form.FormType == FormType.Horizontal) ? new { @class = "col-sm-offset-2 col-sm-10" } : null;
            var help = form.BootEx.HelpBlockFor(expression, helpAttr);

            group.InnerHtml = label.ToHtmlString() + input + help;

            form.BootEx.Html.ViewContext.Writer.Write(group.ToString());
        }

        public static void DropDownListRowFor<TModel, TProperty>(this BootExForm<TModel> form,
           Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            string optionLabel = null,
            object htmlAttributes = null)
        {
            var group = new TagBuilder("div");
            group.AddCssClass("form-group");
            var htmlAttr = HtmlUtil.AddCssClass(htmlAttributes, "form-control");

            var labelClassCss = "control-label";

            if (form.FormType == FormType.Horizontal)
            {
                labelClassCss += " col-sm-2";
            }

            var label = form.BootEx.Html.LabelFor(expression, new { @class = labelClassCss });
            var input = form.BootEx.Html.DropDownListFor(expression, selectList, optionLabel, htmlAttr);

            if (form.FormType == FormType.Horizontal)
            {
                var inputWrapper = new TagBuilder("div");
                inputWrapper.AddCssClass("col-sm-10");
                input = MvcHtmlString.Create(inputWrapper.ToString(TagRenderMode.StartTag) +
                                             input +
                                             inputWrapper.ToString(TagRenderMode.EndTag));
            }

            var helpAttr = (form.FormType == FormType.Horizontal) ? new { @class = "col-sm-offset-2 col-sm-10" } : null;
            var help = form.BootEx.HelpBlockFor(expression, helpAttr);

            group.InnerHtml = label.ToHtmlString() + input + help;
            form.BootEx.Html.ViewContext.Writer.Write(group.ToString());
        }
    }
}