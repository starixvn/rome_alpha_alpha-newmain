namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class ReportMenuItem : MenuItem
    {
        public ReportMenuItem()
        {
        }

        public override bool IsVisible
        {
            get
            {
                return UserSession.Current.SubPageAccess.ViewReports;
            }
        }
    }
}