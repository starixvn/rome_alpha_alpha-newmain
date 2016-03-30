namespace Alpha.Main.Site.Services.Impl
{
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Entities;
    using Fanex.Razorbill.Security.Sessions;
    using System;
    using System.Linq;

    public class SecurityCodeServiceImpl : SecurityCodeService
    {
        public bool IsActive
        {
            get
            {
                return SecurityCodeConfig.IsActive;
            }
        }

        public bool IsRequireSecurityCode
        {
            get
            {
                return SecurityCodeSession.IsRequireSecurityCode;
            }
        }

        public bool IsInternal
        {
            get
            {
                return SecurityCodeSession.IsInternal;
            }
        }

        public SecurityCodeStatus SecurityCodeResult
        {
            get
            {
                return (SecurityCodeStatus)SecurityCodeSession.SecurityCodeResult;
            }
        }

        private ISecurityCodeSession SecurityCodeSession
        {
            get
            {
                return CookieLessSessionFactory.SecurityCodeSession;
            }
        }

        public bool IsIgnoreSecurityCode(string url)
        {
            return SecurityCodeConfig.IsIgnoreSecurityCode(url);
        }

        public bool IsForceUseSecurityCode(string site)
        {
            var isNonUseSecurityCodeSite = AppSettings.NonUseSecurityCodeSites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str.ToLower())
                .Contains(site.ToLower());
            bool isForceUseSecurityCode = !isNonUseSecurityCodeSite &&
                SecurityCodeConfig.IsActive &&
               (IsRequireSecurityCode ||
               SecurityCodeResult == SecurityCodeStatus.FORCED_CHANGE_SC ||
               (SecurityCodeResult == SecurityCodeStatus.PROVIDED_SC_SUCCESS && !IsInternal));

            return isForceUseSecurityCode;
        }
    }
}