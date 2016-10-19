namespace Alpha.Main.Criteria.Otp
{
    using System;
    using Fanex.Data.Repository;

    public class DeleteCustOtpSecretKeyCommand : NonQueryCommand
    {
        public int UserID { get; set; }

        public int CustID { get; set; }

        public int SubAccID { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_OTP_CustomerOTPDel";
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