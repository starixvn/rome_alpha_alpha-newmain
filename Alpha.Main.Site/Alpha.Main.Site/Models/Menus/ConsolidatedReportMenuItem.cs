namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ConsolidatedReportMenuItem : MenuItemBase
    {
        public ConsolidatedReportMenuItem(string key, string url, string function, bool isVisible, bool showSubMenus)
            : base(key, url, function, isVisible, showSubMenus)
        {
        }
    }
}