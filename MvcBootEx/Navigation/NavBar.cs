using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcBootEx.Navigation
{
    public class NavBar : HtmlElement
    {
        private HtmlElement _menuWrapper;

        public NavBar(BootEx bootex)
            : base(bootex, "nav")
        {
        }

        public MvcHtmlString Brand(string brandTitle, string brandUrl = null, object htmlAttributes = null)
        {
            if (brandTitle.IsEmpty())
                return MvcHtmlString.Empty;

            var builder = new TagBuilder("div");
            builder.AddCssClass("navbar-header");
            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            TagBuilder builderLink;

            if (brandUrl.IsEmpty())
            {
                builderLink = new TagBuilder("span");
                builderLink.AddCssClass("navbar-brand");
                builderLink.SetInnerText(brandTitle);
            }
            else
            {
                builderLink = new TagBuilder("a");
                builderLink.AddCssClass("navbar-brand");
                builderLink.SetInnerText(brandTitle);
                builderLink.MergeAttribute("href", brandUrl);
            }

            var mobileHtml = new StringBuilder();
            mobileHtml.Append("<button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\"#bs-example-navbar-collapse-1\">");
            mobileHtml.Append("<span class=\"sr-only\">Toggle navigation</span>");
            mobileHtml.Append("<span class=\"icon-bar\"></span>");
            mobileHtml.Append("<span class=\"icon-bar\"></span>");
            mobileHtml.Append("<span class=\"icon-bar\"></span>");
            mobileHtml.Append("</button>");

            builder.InnerHtml = mobileHtml + builderLink.ToString();
            ViewContext.Writer.Write(builder.ToString());

            return MvcHtmlString.Create(builder.ToString());
        }

        public NavBarMenu BeginMenu(Position position, object htmlAttributes = null)
        {
            WrapMenu();
            var bar = new NavBarMenu(BootEx);
            bar.MergeAttributes(htmlAttributes);
            string cssClass = "nav navbar-nav";

            switch (position)
            {
                case Position.Right:
                    cssClass += " navbar-right";
                    break;
                case Position.Left:
                    cssClass += " navbar-left";
                    break;
            }

            bar.AddCssClass(cssClass);
            bar.Begin();

            return bar;
        }

        private void WrapMenu()
        {
            if (_menuWrapper != null)
                return;

            _menuWrapper = new HtmlElement(BootEx, "div",
                new {@class = "collapse navbar-collapse", @id = "bs-example-navbar-collapse-1"});
        }
    }
}