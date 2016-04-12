namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Web;
    using Main.Helpers;
    using Main.Models.Enumerations;
    using Sunplus.Agent.Utilities;

    public class IndexViewModel
    {
        private readonly int custId;
        private readonly int onlineUserId;
        private readonly byte roleId;
        private readonly string language;
        private readonly string languageKey;
        private readonly Func<string> getDefaultUrl;
        private readonly string passwordHash;

        public IndexViewModel(
            int custId,
            int onlineUserId,
            byte roleId,
            string language,
            string languageKey,
            Func<string> getDefaultUrl)
        {
            this.custId = custId;
            this.roleId = roleId;
            this.language = language;
            this.languageKey = languageKey;
            this.onlineUserId = onlineUserId;
            this.getDefaultUrl = getDefaultUrl;
            this.passwordHash = Encryption.RandomString(new Random().Next(4, 8));
        }

        public int CustId
        {
            get
            {
                return this.custId;
            }
        }

        public int OnUserId
        {
            get
            {
                return this.onlineUserId;
            }
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
                return MicroSites.OldMainUrl("~/");
            }
        }

        public string HowToUseRootPath
        {
            get
            {
                return MicroSites.HowToUseUrl("~/");
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
                return this.passwordHash;
            }
        }
    }
}