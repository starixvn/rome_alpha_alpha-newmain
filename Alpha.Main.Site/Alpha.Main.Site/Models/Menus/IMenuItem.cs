namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMenuItem
    {
        string Key { get; set; }

        string Name { get; set; }

        string DisplayName { get; }

        string Url { get; set; }

        string AppName { get; set; }

        MenuStatus Status { get; }

        bool ShowSubMenus { get; set; }

        List<IMenuItem> SubMenus { get; set; }
    }
}
