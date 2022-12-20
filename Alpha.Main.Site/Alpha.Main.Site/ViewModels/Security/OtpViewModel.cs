namespace Alpha.Main.Site.ViewModels.Security
{
    using Alpha.Main.Site.Helpers;
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using Main.Models.Otp;

    public class OtpViewModel
    {
        public CustomerOtp CustomerOtp { get; set; }

        public bool IsAlreadySetup { get; set; }

        public bool IsShowNicknameLink { get; set; }

        public bool IsOfferProtectedMode { get; set; }

        public RegisterAccountViewModel RegisterAccountViewModel { get; set; }

        public bool EnableSecurityCode { get; set; }

        public string MainRootPath => UrlHelper.MapMainPath("~/");
    }
}