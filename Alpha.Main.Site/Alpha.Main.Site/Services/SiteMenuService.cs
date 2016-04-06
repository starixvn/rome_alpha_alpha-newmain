namespace Alpha.Main.Site.Services
{
    using System.Collections.Generic;
    using Models.Enumerations;
    using Models.Menus;

    public interface SiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUserLevel(UserLevel userLevel);
    }
}
