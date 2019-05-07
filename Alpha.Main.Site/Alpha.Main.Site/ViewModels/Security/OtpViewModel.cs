namespace Alpha.Main.Site.ViewModels.Security
{
    using Main.Models.Otp;

    public class OtpViewModel
    {
        public CustomerOtp CustomerOtp { get; set; }

        public bool IsAlreadySetup { get; set; }

        public bool IsShowNicknameLink { get; set; }
    }
}