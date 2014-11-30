using System.Web.Mvc;

namespace MvcBootEx
{
    public class BootEx
    {
        public HtmlHelper Html { get; private set; }

        internal BootEx(HtmlHelper helper)
        {
            Html = helper;
        }
    }

    public class BootEx<TModel> : BootEx
    {
        public new HtmlHelper<TModel> Html { get; private set; }

        internal BootEx(HtmlHelper<TModel> helper)
            : base(helper)
        {
            Html = helper;
        }
    }
}