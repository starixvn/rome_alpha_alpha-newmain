namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class ViewLogMenuItem : MenuItemBase
    {
        public ViewLogMenuItem(string key, string url, string function, bool isVisible, bool showSubMenus)
            : base(key, url, function, isVisible, showSubMenus)
        {
        }

        public override bool IsVisible
        {
            get
            {
                return UserSession.Current.SubPageAccess.ViewLog;
            }
        }
    }
}