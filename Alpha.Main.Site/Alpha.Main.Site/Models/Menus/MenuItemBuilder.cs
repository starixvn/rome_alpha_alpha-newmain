namespace Alpha.Main.Site.Models.Menus
{
    using System.Web;

    public interface MenuItemBuilder
    {
        IMenuItem BuildMenuItem(SiteMapNode node);
    }
}
