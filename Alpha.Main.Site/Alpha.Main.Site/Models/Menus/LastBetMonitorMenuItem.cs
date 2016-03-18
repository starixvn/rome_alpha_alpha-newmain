namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Linq;
    using Services;
    using Sunplus.Agent.Model.Entities;

    public class LastBetMonitorMenuItem : MenuItem
    {
        //public override bool IsDisabled
        //{
        //    get
        //    {
        //        if (UserSession.Current.IsInternal || AppSettings.EnableBetsListMonitoring)
        //        {
        //            return false;
        //        }

        //        var usersAlwaysToAccessThisFunction = AppSettings.UsersAlwaysAllowAccessBetsListMonitoring;
        //        var isDisabled = string.IsNullOrEmpty(usersAlwaysToAccessThisFunction) ||
        //            !usersAlwaysToAccessThisFunction.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
        //            .ToList()
        //            .Contains(UserSession.Current.CustId.ToString());
        //        return isDisabled;
        //    }
        //}
    }
}