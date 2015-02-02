using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MvcBootEx.Navigation
{
    public class MenuHtmlElement : HtmlElement
    {
        private List<NavItem> _items;

        public MenuHtmlElement(BootEx bootex)
            : base(bootex, "ul")
        {
            _items = new List<NavItem>();
        }

        public MenuHtmlElement Item(NavItem item)
        {
            AddItem(item);
            return this;
        }

        public NavItem Items(NavItem item)
        {
            AddItem(item);
            return item;
        }

        private void AddItem(NavItem item)
        {
            item.IsActive = (LinkEx.IsCurrentRoute(item.Action, item.Controller, item.RouteValues, true));
            _items.Add(item);
        }

        private bool ExistsActiveItem
        {
            get { return _items.Any(x => x.IsActive); }
        }

        protected void WriteItems()
        {
            var existsActive = ExistsActiveItem;

            foreach (var item in _items)
            {
                WriteItem(item, existsActive);
            }
        }

        protected void WriteItem(NavItem item, bool existsActive)
        {
            var li = new TagBuilder("li");
            li.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(item.HtmlAttributes));

            var link = BootEx.ActiveLink(item.LinkText, item.Action, item.Controller, item.RouteValues,
                item.HtmlAttributes, item.Icons).ToString();

            if (!item.IsActive && !existsActive)
            {
                item.IsActive = LinkEx.IsCurrentRoute(item.Action, item.Controller, item.RouteValues, false);
                existsActive = item.IsActive;
            }

            if (item.IsActive)
            {
                li.AddCssClass("active");
            }

            ViewContext.Writer.Write(li.ToString(TagRenderMode.StartTag));
            ViewContext.Writer.Write(link);

            if (item.ChildItems.Any())
            {
                var ul = new TagBuilder("ul");
                ul.AddCssClass("sub");
                ViewContext.Writer.Write(ul.ToString(TagRenderMode.StartTag));

                foreach (var child in item.ChildItems)
                {
                    WriteItem(child, existsActive);
                }

                ViewContext.Writer.Write(ul.ToString(TagRenderMode.EndTag));
            }

            ViewContext.Writer.Write(li.ToString(TagRenderMode.EndTag));
        }
    }
}
