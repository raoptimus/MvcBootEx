using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
                if (item.ChildItems.Any()) //TODO dropdown menu
                {

                }
                else
                {
                    var tag = new TagBuilder("li");
                    tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(item.HtmlAttributes));

                    var link = BootEx.ActiveLink(item.LinkText, item.Action, item.Controller, item.RouteValues,
                        item.HtmlAttributes, item.Icons).ToString();

                    if (!item.IsActive && !existsActive)
                    {
                        item.IsActive = LinkEx.IsCurrentRoute(item.Action, item.Controller, item.RouteValues, false);
                        existsActive = item.IsActive;
                    }

                    if (item.IsActive)
                    {
                        tag.AddCssClass("active");
                    }

                    tag.InnerHtml = link;

                    ViewContext.Writer.Write(tag.ToString());
                }
            }
        }
    }
}
