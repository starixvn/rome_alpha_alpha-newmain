namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Linq;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class LastBetMonitorMenuItem : MenuItem
    {
        public LastBetMonitorMenuItem(Func<MenuStatus> getMenuStatusFunc)
        {
            this.GetMenuStatusFunc = getMenuStatusFunc;
        }
    }
}