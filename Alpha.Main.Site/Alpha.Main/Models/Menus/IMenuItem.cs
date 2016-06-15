namespace Alpha.Main.Models.Menus
{
    using System.Collections.Generic;

    public interface IMenuItem
    {
        string Key { get; set; }

        string ResourceKey { get; set; }

        string DisplayName { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed!")]
        string Url { get; set; }

        string AppPath { get; set; }

        bool ByPassAuthorization { get; set; }

        MenuStatus Status { get; }

        bool ShowSubMenus { get; set; }

        List<IMenuItem> SubMenus { get; set; }

        IEnumerable<IMenuItem> VisibileSubMenus { get; }
    }
}
