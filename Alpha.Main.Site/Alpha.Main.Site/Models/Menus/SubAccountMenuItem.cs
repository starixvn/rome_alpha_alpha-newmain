namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Model.Entities.Permission;

    public class SubAccountMenuItem : MenuItem
    {
        public SubAccountMenuItem(Func<MenuStatus> getMenuStatusFunc)
        {
            this.GetMenuStatusFunc = getMenuStatusFunc;
        }
    }
}