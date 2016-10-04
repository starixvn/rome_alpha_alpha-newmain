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
        private readonly Func<string> getDefaultUrl;
        private readonly Func<bool> isShowIntroduction;

        public IndexViewModel(IndexQuery message, Func<string> getDefaultUrl, Func<bool> isShowIntroduction)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            UserLevel = Enumeration.FromValue<UserLevel>(message.RoleId).DisplayName.ToLower();
            IsShowIntroduction = isShowIntroduction();
            DefaultUrl = getDefaultUrl();
            roleId = message.RoleId;            
            CustId = message.CustId;
            Language = message.Language;
            LanguageKey = message.LanguageKey;
            OnUserId = message.OnUserId;
            SubPageAccess = message.SubPageAccess;
            EnableSecurityCode = message.EnableSecurityCode;
            IsInternalWebVPN = message.IsInternalWebVPN;
            IsSubAccount = message.IsSubAccount;
            this.isShowIntroduction = isShowIntroduction ?? FunctionRegistry.ShowIntroduction;
            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
        }

        public bool EnableSecurityCode { get; private set; }

        public int CustId { get; private set; }

        public int OnUserId { get; private set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(roleId).DisplayName.ToLower();
            }
        }

        public string Language { get; private set; }

        public string LanguageKey { get; private set; }

        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Reviewed!")]
        public string DefaultUrl
        {
            get
            {
                return getDefaultUrl?.Invoke() ?? MainRootPath;
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

        public bool IsShowIntroduction
        {
            get
            {
                return isShowIntroduction?.Invoke() ?? true;
            }
        }
        public HeaderNavViewModel HeaderNavViewModel
        {
            get
            {
                return new HeaderNavViewModel(CustId, EnableSecurityCode);
            }
        }
    }
}