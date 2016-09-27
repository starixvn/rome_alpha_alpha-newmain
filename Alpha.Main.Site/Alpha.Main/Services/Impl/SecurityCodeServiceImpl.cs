namespace Alpha.Main.Services.Impl
{
    using Alpha.Main.Services;
    using Criteria.SecurityCode;
    using DataAccesses;
    using DataAccesses.Impl;
    using Models.SecurityCode;
    using Params.SecurityCode;

    public class SecurityCodeServiceImpl : SecurityCodeService
    {
        private readonly SecurityCodeDataAccess securityCodeDataAccess;

        public SecurityCodeServiceImpl(SecurityCodeDataAccess securityCodeDataAccess = null)
        {
            this.securityCodeDataAccess = securityCodeDataAccess ?? new SecurityCodeDataAccessImpl();
        }

        public string GetMainAccountHashSecurityCode(int custId)
        {
            return this.securityCodeDataAccess.GetMainAccountHashSecurityCode(
                new GetMainAccountHashSecurityCodeCriteria
                {
                    CustId = custId
                });
        }

        public string GetSubAccountHashSecurityCode(int custId, int subAccountId)
        {
            return this.securityCodeDataAccess.GetSubAccountHashSecurityCode(
                new GetSubAccountHashSecurityCodeCriteria
                {
                    CustId = custId,
                    SubaccId = subAccountId
                });
        }

        public Result UpdateMainAccountHashSecurityCodeCounter(SecurityCodeParams securityCodeParams)
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

        public Result UpdateSubAccountHashSecurityCodeCounter(SecurityCodeParams securityCodeParams)
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

        public Result UpdateMainAccountHashSecurityCode(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.UpdateMainAccountHashSecurityCode(
                 new UpdateMainAccountHashSecurityCodeCriteria
                 {
                     CustId = securityCodeParams.CustId,
                     SubaccId = securityCodeParams.SubAccountId,
                     UserId = securityCodeParams.UserId,
                     HashSecurityCode = securityCodeParams.HashSecurityCode,
                     IP = securityCodeParams.IP,
                     Language = securityCodeParams.Language
                 });
        }

        public Result UpdateSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.UpdateSubAccountHashSecurityCode(new UpdateSubAccountHashSecurityCodeCriteria
            {
                CustId = securityCodeParams.CustId,
                SubaccId = securityCodeParams.SubAccountId,
                UserId = securityCodeParams.UserId,
                HashSecurityCode = securityCodeParams.HashSecurityCode,
                IP = securityCodeParams.IP,
                Language = securityCodeParams.Language
            });
        }

        public Result ResetSubAccountHashSecurityCode(SecurityCodeParams securityCodeParams)
        {
            return this.securityCodeDataAccess.ResetSubAccountHashSecurityCode(new ResetSubAccountHashSecurityCodeCriteria
            {
                CustId = securityCodeParams.CustId,
                SubaccId = securityCodeParams.SubAccountId,
                UserId = securityCodeParams.UserId,
                HashSecurityCode = securityCodeParams.HashSecurityCode,
                IP = securityCodeParams.IP,
                Language = securityCodeParams.Language
            });
        }
    }
}