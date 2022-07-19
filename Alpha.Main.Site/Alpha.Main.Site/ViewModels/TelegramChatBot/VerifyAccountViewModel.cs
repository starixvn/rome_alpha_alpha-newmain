namespace Alpha.Main.Site.ViewModels.TelegramChatBot
{
    public class VerifyAccountViewModel
    {
        public VerifyAccountViewModel(
            string url, 
            string qrCodeDataUrl,
            bool isShowCaptcha = false)
        {
            Url = url;
            QRCodeDataUrl = qrCodeDataUrl;
            IsShowCaptcha = isShowCaptcha;
        }

        public string Url { get; }

        public string QRCodeDataUrl { get; }

        public bool IsShowCaptcha { get; }
    }
}