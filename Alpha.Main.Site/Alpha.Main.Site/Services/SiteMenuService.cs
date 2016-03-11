namespace Alpha.Main.Site.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models;
    using Models.Dtos;

    public interface SiteMenuService
    {
        IEnumerable<MenuItem> GetMenuByUser(User user);
    }
}
