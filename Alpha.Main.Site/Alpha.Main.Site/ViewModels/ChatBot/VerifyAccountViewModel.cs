namespace Alpha.Main.Site.ViewModels.ChatBot
{
    public class VerifyAccountViewModel
    {
        public VerifyAccountViewModel(
            string messengerAccount,
            bool isShowCaptcha = false)
        {
            MessengerAccount = messengerAccount;
            IsShowCaptcha = isShowCaptcha;
        }

        public string MessengerAccount { get; }

        public bool IsShowCaptcha { get; }
    }
}