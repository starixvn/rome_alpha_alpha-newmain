namespace Alpha.Main.Site.Models.Menus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class StatementMenuItem : MenuItem
    {
        public StatementMenuItem()
        {
        }

        public override string Url
        {
            get
            {
                var result = base.Url;
                return result.Replace("custidParam", UserSession.Current.CustId.ToString());
            }

            set
            {
                base.Url = value;
            }
        }
    }
}