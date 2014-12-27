using System.Web.Mvc;

namespace MvcBootEx
{
    public static class HtmlEx
    {
        public static BootEx<TModel> BootEx<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new BootEx<TModel>(htmlHelper);
        }

        public static BootEx BootEx(this HtmlHelper htmlHelper)
        {
            return new BootEx(htmlHelper);
        }
    }
}