namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading;
    using System.Web;
    using Sunplus.Agent.Model.Entities;

    public class HeaderNavViewModel
    {
       public string LanguageKey
        {
            get
            {
                return UserSession.Current.LangKey;
            }
        }
    }
}