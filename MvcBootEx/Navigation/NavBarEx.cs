namespace MvcBootEx.Navigation
{
    public static class NavBarEx
    {
        public static NavBar BeginNavBar(this BootEx bootex, NavBarColor color, object htmlAttributes = null)
        {
            return HelperNavMenu(bootex, NavBarType.NoFixed, color, htmlAttributes);
        }

        public static NavBar BeginNavBarFixed(this BootEx bootex, NavBarType type, NavBarColor color, object htmlAttributes = null)
        {
            return HelperNavMenu(bootex, type, color, htmlAttributes);
        }

        internal static NavBar HelperNavMenu(this BootEx bootex, NavBarType type, NavBarColor color, object htmlAttributes)
        {
            var navBar = new NavBar(bootex);
            navBar.AddCssClass("navbar");
            navBar.AddAttribute("role", "navigation");
            navBar.MergeAttributes(htmlAttributes);

            switch (color)
            {
                case NavBarColor.Default:
                    navBar.AddCssClass("navbar-default");
                    break;
                case NavBarColor.Inverse:
                    navBar.AddCssClass("navbar-inverse");
                    break;
            }

            switch (type)
            {
                case NavBarType.NoFixed:
                    break;
                case NavBarType.FixedToTop:
                    navBar.AddCssClass("navbar-fixed-top");
                    break;
                case NavBarType.FixedToBottom:
                    navBar.AddCssClass("navbar-fixed-bottom");
                    break;
                case NavBarType.FixedStaticToTop:
                    navBar.AddCssClass("navbar-static-top");
                    break;
                case NavBarType.FixedStaticToBottom:
                    navBar.AddCssClass("navbar-static-bottom");
                    break;
            }

            navBar.Begin("<div class=\"container-fluid\">", "</div>");
            return navBar;
        }
    }
}