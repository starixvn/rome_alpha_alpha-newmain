namespace Alpha.Main.Models.Menus
{
    using System.Collections.Generic;

    public interface IMenuItem
    {
        string Key { get; set; }

        string ResourceKey { get; set; }

        string DisplayName { get; }

        string Url { get; set; }

        string AppPath { get; set; }

        MenuStatus Status { get; }

        bool ShowSubMenus { get; set; }

        List<IMenuItem> SubMenus { get; set; }

        IEnumerable<IMenuItem> VisibileSubMenus { get; }
    }
}
