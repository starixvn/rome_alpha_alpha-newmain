namespace Alpha.Main.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Models.Menus;
    using Sunplus.Agent.Model.Entities;

    public static class MenuItemConfig
    {
        public static void Register()
        {
            TotalBetsMenuItem.CheckVisibleFunc = () => UserSession.Current.SubPageAccess.ViewTotalBets;
            ReportMenuItem.CheckVisibleFunc = () => UserSession.Current.SubPageAccess.ViewReports;
        }
    }
}