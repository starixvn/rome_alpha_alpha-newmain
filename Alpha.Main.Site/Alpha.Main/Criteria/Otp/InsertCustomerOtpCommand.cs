namespace Alpha.Main.Criteria.Otp
{
    using System;
    using Fanex.Data.Repository;

    public class InsertCustomerOtpCommand : NonQueryCommand
    {
        public InsertCustomerOtpCommand()
        {
            ErrCode = 0;
            ErrMsg = string.Empty;
        }

        public int CustID { get; set; }

        public int SubAccID { get; set; }

        public string SecretKey { get; set; }

        public string DeviceInfo { get; set; }

        public string Language { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "Age_OTP_CustomerOTPIns";
        }

        public override bool IsValid()
        {
            return CustID > 0
               && SubAccID >= 0
               && !string.IsNullOrWhiteSpace(Language)
               && !string.IsNullOrWhiteSpace(IP)
               && !string.IsNullOrWhiteSpace(DeviceInfo)
               && !string.IsNullOrWhiteSpace(SecretKey);
        }
    }
}