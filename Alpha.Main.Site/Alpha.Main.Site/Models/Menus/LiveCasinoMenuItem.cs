namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities.Permission;

    public class LiveCasinoMenuItem : MenuItem
    {
        public LiveCasinoMenuItem()
        {
        }

        public override bool IsVisible
        {
            get
            {
                return UserPermission.IsShowLiveCasino;
            }
        }
    }
}