namespace Alpha.Main.Criteria.Otp
{
    using System;
    using Fanex.Data.Repository;

    public class GetCustomerOtpCriteria : CriteriaBase
    {
        public int CustID { get; set; }

        public int SubAccID { get; set; }

        public override string GetSettingKey()
        {
            return "Age_OTP_CustomerOTPSel";
        }

        public override bool IsValid()
        {
            return CustID > 0
               && SubAccID >= 0;
        }
    }
}