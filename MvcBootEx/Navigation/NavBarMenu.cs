namespace MvcBootEx.Navigation
{
    public class NavBarMenu : MenuHtmlElement
    {
        public NavBarMenu(BootEx bootex) : base(bootex)
        {
        }

        public override void Dispose()
        {
            WriteItems();
            base.Dispose();
        }
    }
}