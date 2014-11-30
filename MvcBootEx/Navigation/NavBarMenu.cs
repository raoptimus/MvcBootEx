namespace MvcBootEx.Navigation
{
    public class NavBarMenu : HtmlElement
    {
        public NavBarMenu(BootEx bootEx)
            : base(bootEx, "ul")
        {

        }

        public void EndBarMenu()
        {
            End();
        }
    }
}