namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities.Permission;

    public class BingoMenuItem : MenuItem
    {
        public BingoMenuItem(Func<bool> checkVisibleFunc)
        {
            this.checkVisibleFunc = checkVisibleFunc;
        }
    }
}