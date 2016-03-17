namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities.Permission;

    public class NumberGameMenuItem : MenuItem
    {
        public NumberGameMenuItem()
        {
        }

        public override bool IsVisible
        {
            get
            {                
                return UserPermission.IsShowNumberGame;
            }
        }
    }
}