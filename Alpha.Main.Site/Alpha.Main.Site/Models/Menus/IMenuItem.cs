namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IMenuItem
    {
        string Key { get; }
        string DisplayName { get; }
        string Url { get; }
        string Function { get;}        
        bool IsVisible { get; }
        bool ShowSubMenus { get; }
        List<IMenuItem> SubMenus { get; set; }       
    }
}
