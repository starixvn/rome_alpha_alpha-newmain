namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities.Permission;

    public class RacingMenuItem : MenuItemBase
    {
        public RacingMenuItem(string key, string url, string function, bool isVisible, bool showSubMenus)
            : base(key, url, function, isVisible, showSubMenus)
        {
        }

        public override bool IsVisible
        {
            get
            {
                return UserPermission.IsShowRacing;
            }
        }
    }
}