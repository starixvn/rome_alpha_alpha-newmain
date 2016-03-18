namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class MemberInfoMenuItem : MenuItem
    {
        public MemberInfoMenuItem(Func<bool> checkVisibleFunc)
        {
            this.checkVisibleFunc = checkVisibleFunc;
        }        
    }
}