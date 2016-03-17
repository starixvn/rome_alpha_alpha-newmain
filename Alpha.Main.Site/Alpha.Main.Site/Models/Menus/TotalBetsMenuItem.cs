namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class TotalBetsMenuItem : MenuItem
    {
        public TotalBetsMenuItem()
        {
        }

        public override bool IsVisible
        {
            get
            {
                return UserSession.Current.SubPageAccess.ViewTotalBets;
            }
        }
    }
}