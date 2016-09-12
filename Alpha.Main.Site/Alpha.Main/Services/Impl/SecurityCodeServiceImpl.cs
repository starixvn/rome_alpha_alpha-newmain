namespace Alpha.Main.Services.Impl
{
    using Alpha.Main.Services;
    using Configurations;
    using Criteria.SecurityCode;
    using DataAccesses;
    using DataAccesses.Impl;
    using Models.SecurityCode;
    using Params.SecurityCode;
    using Sessions;
    using System;
    using System.Linq;

    public class SecurityCodeServiceImpl : SecurityCodeService
    {

        private readonly SecurityCodeDataAccess securityCodeDataAccess;

        public SecurityCodeServiceImpl(SecurityCodeDataAccess securityCodeDataAccess = null)
        {
            this.securityCodeDataAccess = securityCodeDataAccess ?? new SecurityCodeDataAccessImpl();
        }

        public bool IsIgnoreSecurityCode(string url)
        {
            return SecurityCodeConfig.IsIgnoreSecurityCode(url);
        }

        public bool IsForceUseSecurityCode(string site, SecurityCodeStatus securityCodeResult, ISecurityCodeSession securityCodeSession)
        {
            if (SecurityCodeConfig.IsActive)
            {
                return false;
            }

            if (site == null)
            {
                throw new ArgumentNullException(nameof(site), "Site cannot be null");
            }

            var isNonUseSecurityCodeSite = AppSettings.NonUseSecurityCodeSites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(str => str.ToLower())
                .Contains(site.ToLower());

            if (isNonUseSecurityCodeSite)
            {
                return false;
            }

            if (securityCodeSession == null)
            {
                throw new ArgumentNullException(nameof(securityCodeSession), "Security Code Session cannot be null");
            }

            bool isForceUseSecurityCode = securityCodeSession.IsRequireSecurityCode ||
               securityCodeResult == SecurityCodeStatus.FORCED_CHANGE_SC ||
               (securityCodeResult == SecurityCodeStatus.PROVIDED_SC_SUCCESS && !securityCodeSession.IsInternal);

            return isForceUseSecurityCode;
        }

        public string GetMainAccountSecurityCode(int custId)
        {
            return this.securityCodeDataAccess.GetMainAccountSecurityCode(
                new GetMainAccountSecurityCodeCriteria
                {
                    CustId = custId
                });
        }

        public string GetSubAccountSecurityCode(int custId, int subAccountId)
        {
            return this.securityCodeDataAccess.GetSubAccountSecurityCode(
                new GetSubAccountSecurityCodeCriteria
                {
                    CustId = custId,
                    SubaccId = subAccountId
                });
        }

        public Result UpdateMainAccountSecurityCodeCounter(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.UpdateMainAccountSecurityCodeCounter(
                new UpdateMainAccountSecurityCodeCounterCriteria
                {
                    CustId = securityCodeParams.CustId,
                    CheckedSecurityCode = securityCodeParams.IsValidSecurityCode,
                    IP = securityCodeParams.IP,
                    Language = securityCodeParams.Language
                });
        }

        public Result UpdateSubAccountSecurityCodeCounter(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.UpdateSubAccountSecurityCodeCounter(
                new UpdateSubAccountSecurityCodeCounterCriteria
                {
                    CustId = securityCodeParams.CustId,
                    SubaccId = securityCodeParams.SubAccountId,
                    CheckedSecurityCode = securityCodeParams.IsValidSecurityCode,
                    IP = securityCodeParams.IP,
                    Language = securityCodeParams.Language
                });
        }

        public Result UpdateMainAccountSecurityCode(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.UpdateMainAccountSecurityCode(
                 new UpdateMainAccountSecurityCodeCriteria
                 {
                     CustId = securityCodeParams.CustId,
                     SubaccId = securityCodeParams.SubAccountId,
                     UserId = securityCodeParams.UserId,
                     HashSecurityCode = securityCodeParams.HashSecurityCode,
                     IP = securityCodeParams.IP,
                     Language = securityCodeParams.Language
                 });
        }

        public Result UpdateSubAccountSecurityCode(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.UpdateSubAccountSecurityCode(new UpdateSubAccountSecurityCodeCriteria
            {
                CustId = securityCodeParams.CustId,
                SubaccId = securityCodeParams.SubAccountId,
                UserId = securityCodeParams.UserId,
                HashSecurityCode = securityCodeParams.HashSecurityCode,
                IP = securityCodeParams.IP,
                Language = securityCodeParams.Language
            });
        }

        public Result ResetSubAccountSecurityCode(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.ResetSubAccountSecurityCode(new ResetSubAccountSecurityCodeCriteria
            {
                CustId = securityCodeParams.CustId,
                SubaccId = securityCodeParams.SubAccountId,
                UserId = securityCodeParams.UserId,
                HashSecurityCode = securityCodeParams.SecurityCode,
                IP = securityCodeParams.IP,
                Language = securityCodeParams.Language
            });
        }
    }
}