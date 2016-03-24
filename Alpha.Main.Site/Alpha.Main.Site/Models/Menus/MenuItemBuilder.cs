namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;

    public interface MenuItemBuilder
    {
        IMenuItem BuildMenuItem(SiteMapNode node);
    }
}
