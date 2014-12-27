using System.Web;

namespace MvcBootEx.Navigation
{
    public class Nav : MenuHtmlElement
    {
        public Nav(BootEx bootex) : base(bootex)
        {
        }

        public void SiteMap(SiteMapProvider sitemap)
        {
            
        }

        public override void Dispose()
        {
            WriteItems();
            base.Dispose();
        }
    }
}