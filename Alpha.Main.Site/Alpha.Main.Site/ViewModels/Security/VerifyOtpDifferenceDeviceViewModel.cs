namespace Alpha.Main.Site.ViewModels.Security
{
    public class VerifyOtpDifferenceDeviceViewModel
    {
        public string Username { get; set; }

        public string AlternateUsername { get; set; }

        public string UsernameShower { get; set; }

        public bool IsShowGenerateOTPButton { get; set; }

        public long OTPExpirationTime { get; set; }
    }
}