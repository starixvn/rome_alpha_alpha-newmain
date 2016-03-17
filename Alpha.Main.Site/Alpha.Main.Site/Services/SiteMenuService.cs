namespace Alpha.Main.Site.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Models.Dtos;
    using Models.Menus;

    public interface SiteMenuService
    {
        IEnumerable<IMenuItem> GetMenuByUser(IUser user);
    }
}
