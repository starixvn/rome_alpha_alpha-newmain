namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Web;
    using App_Start;
    using Helpers;
    using Models.Enumerations;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;

    public class IndexViewModel
    {
        private readonly IUserSession userSession;
        private readonly string defaultUrl;

        public IndexViewModel(IUserSession userSession = null, string defaultUrl = "")
        {
            this.userSession = userSession;
            this.defaultUrl = string.IsNullOrWhiteSpace(defaultUrl) ? FunctionRegistry.GetDefaultUrl() : defaultUrl;
        }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(userSession.RoleId).DisplayName.ToLower();
            }
        }

        public string Language
        {
            get
            {
                return userSession.Language;
            }
        }

        public string LanguageKey
        {
            get
            {
                return userSession.LangKey;
            }
        }

        public int OnUserId
        {
            get
            {
                return userSession.OnUserId;
            }
        }

        public string DefaultUrl
        {
            get
            {
                return defaultUrl;
            }
        }

        public string OldMainRootPath
        {
            get
            {
                return AppPathHelper.GetOldMainPath("~/");
            }
        }

        public string MainRootPath
        {
            get
            {
                return AppPathHelper.GetAppPath(HttpRuntime.AppDomainAppVirtualPath, "~/");
            }
        }

        public string PasswordHash
        {
            get
            {
                return Encryption.RandomString(new Random().Next(4, 8));
            }
        }
    }
}
