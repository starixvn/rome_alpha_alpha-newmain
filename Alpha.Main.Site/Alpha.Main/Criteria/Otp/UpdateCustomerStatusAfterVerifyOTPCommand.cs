namespace Alpha.Main.Criteria.Otp
{
    using Fanex.Data.Repository;

    public class UpdateCustomerStatusAfterVerifyOTPCommand : NonQueryCommand
    {
        public int CustID { get; set; }

        public int SubAccID { get; set; }

        public bool CheckedOTP { get; set; }

        public string FunctionName { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_OTP_CustomerStatusUpdAfterVerifiedOTP";
        }

        public override bool IsValid()
        {
            return CustID > 0
                   && SubAccID >= 0
                   && !string.IsNullOrWhiteSpace(Language)
                   && !string.IsNullOrWhiteSpace(IP);
        }
    }
}