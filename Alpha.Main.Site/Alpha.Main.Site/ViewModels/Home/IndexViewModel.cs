namespace Alpha.Main.Site.ViewModels
{
    using Main.Helpers;
    using Main.Models.Enumerations;
    using Models.Requests;
    using Sunplus.Agent.Utilities;
    using System;
    using System.Web;

    public class IndexViewModel
    {
        private readonly int custId;
        private readonly int onlineUserId;
        private readonly int subAccId;
        private readonly byte roleId;
        private readonly string language;
        private readonly string languageKey;
        private readonly Func<string> getDefaultUrl;
        private readonly string passwordHash;
        private readonly string subPageAccess;
        private readonly bool enableSecurityCode;
        private readonly bool isInternalWebVPN;

        public IndexViewModel(IndexQuery message, Func<string> getDefaultUrl)
        {
            this.custId = message.UserId;
            this.roleId = message.RoleId;
            this.language = message.Language;
            this.languageKey = message.LanguageKey;
            this.onlineUserId = message.OnUserId;
            this.subAccId = message.SubAccId;
            this.getDefaultUrl = getDefaultUrl;
            this.passwordHash = Encryption.RandomString(new Random().Next(4, 8));
            this.subPageAccess = message.SubPageAccess;
            this.enableSecurityCode = message.EnableSecurityCode;
            this.isInternalWebVPN = message.IsInternalWebVPN;
        }

        public bool EnableSecurityCode
        {
            get
            {
                return this.enableSecurityCode;
            }
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

        public string SubPageAccess
        {
            get
            {
                return this.subPageAccess;
            }
        }

        public bool IsInternalWebVPN
        {
            get
            {
                return this.isInternalWebVPN;
            }
        }

        public string CurrentURLCookieName
        {
            get
            {
                return CookieHelper.GetCurrentUrlCookieName();
            }
        }
    }
}