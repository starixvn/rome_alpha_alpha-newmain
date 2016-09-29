namespace Alpha.Main.Site.ViewModels
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using Main.Helpers;
    using Main.Models.Enumerations;
    using Models.Requests;
    using Sunplus.Agent.Utilities;

    public class IndexViewModel
    {
        private readonly byte roleId;

        public IndexViewModel(IndexQuery message, Func<string> getDefaultUrl)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            this.roleId = message.RoleId;
            this.IsShowIntroduction = isShowIntroduction();
            this.DefaultUrl = getDefaultUrl();
            this.CustId = message.CustId;
            this.Language = message.Language;
            this.LanguageKey = message.LanguageKey;
            this.OnUserId = message.OnUserId;
            this.SubPageAccess = message.SubPageAccess;
            this.EnableSecurityCode = message.EnableSecurityCode;
            this.IsInternalWebVPN = message.IsInternalWebVPN;
            this.IsSubAccount = message.IsSubAccount;
        }

        public bool EnableSecurityCode { get; private set; }

        public int CustId { get; private set; }

        public int OnUserId { get; private set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.roleId).DisplayName.ToLower();
            }
        }

        public string Language { get; private set; }

        public string LanguageKey { get; private set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed!")]
        public string DefaultUrl { get; private set; }

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

        public string PasswordHash { get; private set; } = Encryption.RandomString(new Random().Next(4, 8));

        public string SubPageAccess { get; private set; }

        public bool IsSubAccount { get; private set; }

        public bool IsInternalWebVPN { get; private set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed!")]
        public string CurrentURLCookieName
        {
            get
            {
                return CookieHelper.GetCurrentUrlCookieName();
            }
        }

        public bool IsShowIntroduction { get; private set; }
        public HeaderNavViewModel HeaderNavViewModel
        {
            get
            {
                return new HeaderNavViewModel(this.CustId, this.EnableSecurityCode);
            }
        }
    }
}