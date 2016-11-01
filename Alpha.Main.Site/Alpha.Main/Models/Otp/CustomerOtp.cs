namespace Alpha.Main.Models.Otp
{
    public class CustomerOtp
    {
        public int CustId { get; set; }

        public string OtpUsername { get; set; }

        public int SubAccId { get; set; }

        public string SecretKey { get; set; }

        public string QrCode { get; set; }

        public bool Enabled { get; set; }

        public string Otp { get; set; }

        public DeviceInfo DeviceInfo { get; set; }

        public string Language { get; set; }
    }
}