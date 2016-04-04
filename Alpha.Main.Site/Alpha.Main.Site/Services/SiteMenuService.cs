namespace Alpha.Main.Site.Services
{
    using System.Collections.Generic;
    using Models;
    using Models.Menus;

    public interface SiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUser(IUser user);
    }
}
