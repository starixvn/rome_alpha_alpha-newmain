namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Models.Enumerations;
    using Sunplus.Agent.Model.Entities;

    public class IndexViewModel
    {
        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(UserSession.Current.RoleId).DisplayName.ToLower();
            }
        }
    }
}