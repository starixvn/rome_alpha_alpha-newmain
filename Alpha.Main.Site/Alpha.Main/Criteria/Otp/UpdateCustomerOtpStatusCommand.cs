namespace Alpha.Main.Criteria.Otp
{
    using Fanex.Data.Repository;

    public class UpdateCustomerOtpStatusCommand : NonQueryCommand
    {
        public int UserID { get; set; }

        public int CustID { get; set; }

        public int SubAccID { get; set; }

        public bool Enable { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_OTP_CustomerOTPStatusUpd";
        }

        public override bool IsValid()
        {
            return CustID > 0
               && SubAccID >= 0
               && UserID > 0
               && !string.IsNullOrWhiteSpace(Language)
               && !string.IsNullOrWhiteSpace(IP);
        }
    }
}