namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Services;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public class FoobetMenuItem : MenuItem
    {
        public FoobetMenuItem()
        {
        }

        public override bool IsVisible
        {
            get
            {
                return UserSession.Current.CustId == AppSettings.CustIdFoobet;
            }
        }
    }
}