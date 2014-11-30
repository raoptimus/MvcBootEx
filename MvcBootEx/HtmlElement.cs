using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcBootEx
{
    public class HtmlElement : IDisposable
    {
        protected readonly BootEx BootEx;
        protected readonly HtmlHelper Html;
        protected readonly ViewContext ViewContext;
        private readonly TagBuilder _tagBuilder;
        private string _innerHtmlEnd;
        private bool _disposed;

        public HtmlElement(BootEx bootex, string tagName, object htmlAttributes = null)
        {
            if (bootex == null || bootex.Html == null || bootex.Html.ViewContext == null)
            {
                throw new ArgumentNullException("bootex");
            }

            BootEx = bootex;
            Html = bootex.Html;
            ViewContext = bootex.Html.ViewContext;
            _tagBuilder = new TagBuilder(tagName);
            MergeAttributes(htmlAttributes);
        }

        public IDictionary<string, string> Attributes
        {
            get { return _tagBuilder.Attributes; }
        }

        public void AddCssClass(string className)
        {
            _tagBuilder.AddCssClass(className);
        }

        public void MergeAttributes(object htmlAttributes)
        {
            _tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public void MergeAttributes(IDictionary<string, object> htmlAttributes)
        {
            _tagBuilder.MergeAttributes(htmlAttributes);
        }

        public void AddAttribute(string name, string value)
        {
            _tagBuilder.Attributes.Add(name, value);    
        }
        public void AddAttribute(KeyValuePair<string, string> item)
        {
            _tagBuilder.Attributes.Add(item);   
        }

        public void Begin(string innerHtmlStart = null, string innerHtmlEnd = null)
        {
            ViewContext.Writer.Write(_tagBuilder.ToString(TagRenderMode.StartTag));

            if (innerHtmlStart != null)
            {
                ViewContext.Writer.Write(innerHtmlStart);
            }

            _innerHtmlEnd = innerHtmlEnd;
        }
        public virtual void Dispose()
        {
            Dispose(true /* disposing */);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (_innerHtmlEnd != null)
            {
                ViewContext.Writer.Write(_innerHtmlEnd);
            }

            ViewContext.Writer.Write(_tagBuilder.ToString(TagRenderMode.EndTag));
        }
        public void End()
        {
            Dispose(true);
        }
    }
}