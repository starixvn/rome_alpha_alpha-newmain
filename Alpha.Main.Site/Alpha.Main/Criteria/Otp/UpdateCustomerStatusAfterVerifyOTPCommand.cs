namespace Alpha.Main.Criteria.Otp
{
    using Fanex.Data.Repository;

    public class UpdateCustomerStatusAfterVerifyOtpCommand : NonQueryCommand
    {
        public int CustId { get; set; }

        public int SubAccId { get; set; }

        public bool CheckedOtp { get; set; }

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
            return CustId > 0
                   && SubAccId >= 0
                   && !string.IsNullOrWhiteSpace(Language)
                   && !string.IsNullOrWhiteSpace(IP);
        }
    }
}