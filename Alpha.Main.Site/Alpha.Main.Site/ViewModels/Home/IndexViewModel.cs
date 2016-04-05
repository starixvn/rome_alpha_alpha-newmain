namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Web;
    using Helpers;
    using Models.Enumerations;
    using Sunplus.Agent.Utilities;

    public class IndexViewModel
    {
        private readonly byte roleId;
        private readonly string language;
        private readonly string languageKey;
        private readonly int onUserId;
        private readonly Func<string> getDefaultUrl;

        public IndexViewModel(byte roleId, string language, string languageKey, int onUserId, Func<string> getDefaultUrl)
        {
            this.roleId = roleId;
            this.language = language;
            this.languageKey = languageKey;
            this.onUserId = onUserId;
            this.getDefaultUrl = getDefaultUrl;
        }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.roleId).DisplayName.ToLower();
            }
        }

        public string Language
        {
            get
            {
                return this.language;
            }
        }

        public string LanguageKey
        {
            get
            {
                return this.languageKey;
            }
        }

        public int OnUserId
        {
            get
            {
                return this.onUserId;
            }
        }

        public string DefaultUrl
        {
            get
            {
                return getDefaultUrl();
            }
        }

        public string OldMainRootPath
        {
            get
            {
                return OldMainSite.Url("~/");
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
