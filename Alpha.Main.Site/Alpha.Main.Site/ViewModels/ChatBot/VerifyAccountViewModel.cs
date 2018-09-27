namespace Alpha.Main.Site.ViewModels.ChatBot
{
    public class VerifyAccountViewModel
    {
        public VerifyAccountViewModel(
            bool isShowCaptcha = false)
        {
            IsShowCaptcha = isShowCaptcha;
        }

        public bool IsShowCaptcha { get; }
    }
}