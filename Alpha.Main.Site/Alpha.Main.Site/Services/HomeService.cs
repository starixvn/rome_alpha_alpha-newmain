namespace Alpha.Main.Site.Services
{
    using System.Collections.Generic;
    using Alpha.Main.Site.Models.Menus;
    using Models.Requests.Home;

    public interface HomeService
    {
        List<IMenuItem> GetNavigation(NavigationQuery query);
    }
}