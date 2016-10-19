namespace Alpha.Main.Models.Otp
{
    using System;

    public class CustomerOtpDto
    {
        public int CustID { get; set; }

        public int SubAccId { get; set; }

        public string SecretKey { get; set; }

        public bool Enabled { get; set; }

        public string DeviceInfo { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}