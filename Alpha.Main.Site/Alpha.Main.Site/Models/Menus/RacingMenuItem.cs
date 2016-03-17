namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities.Permission;

    public class RacingMenuItem : MenuItem
    {
        public RacingMenuItem()
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