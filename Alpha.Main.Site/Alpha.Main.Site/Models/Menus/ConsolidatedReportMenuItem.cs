namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class ConsolidatedReportMenuItem : MenuItem
    {
        public ConsolidatedReportMenuItem()
        {
        }

        public override bool IsVisible
        {
            get
            {
                return UserSession.Current.IsShowConsolidatedReport;
            }
        }
    }
}