namespace Alpha.Main.Criteria.Otp
{
    using System;
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
            throw new NotImplementedException();
        }
    }
}