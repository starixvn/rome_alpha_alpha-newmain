namespace Alpha.Main.Criteria.Otp
{
    using Fanex.Data.Repository;

    public class CheckEnableOtpCommand : NonQueryCommand
    {
        public int CustID { get; set; }

        public int SubAccID { get; set; }

        public bool Enable { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_OTP_CustomerCheckEnableOTP";
        }

        public override bool IsValid()
        {
            return CustID > 0
                && SubAccID >= 0;
        }
    }
}