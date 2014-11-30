
namespace MvcBootEx.Navigation
{
    public static class NavEx
    {
        public static Nav BeginNav(this BootEx bootex, NavType type, object htmlAttributes = null)
        {
            var nav = new Nav(bootex);
            nav.MergeAttributes(htmlAttributes);
            string cssClass = "nav";

            switch (type)
            {
                case NavType.Tabs:
                    cssClass += " nav-tabs";
                    break;
                case NavType.Pills:
                    cssClass += " nav-pills";
                    break;
                case NavType.StackedPills:
                    cssClass += " nav-pills nav-stacked";
                    break;
                case NavType.Justified:
                    cssClass += " nav-justified";
                    break;
            }

            nav.AddCssClass(cssClass);
            nav.Begin();

            return nav;
        }
    }
}