using System.Collections.Generic;

namespace MvcBootEx.Navigation
{
    public class NavItem
    {
        public string LinkText { get; set; }
        public string LinkHref { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public object RouteValues { get; set; }
        public object HtmlAttributes { get; set; }
        public string Icons { get; set; }
        public bool IsActive { get; set; }
        public List<NavItem> ChildItems { get; set; }

        public NavItem()
        {
            ChildItems = new List<NavItem>();
        }

        public NavItem Item(NavItem item)
        {
            ChildItems.Add(item);
            return this;
        }
    }
}