namespace Alpha.Main.Site.ViewModels.ChatBot
{
    public class VerifyAccountViewModel
    {
        public VerifyAccountViewModel(
            int userId,
            string messengerAccount,
            bool isShowCaptcha = false)
        {
            UserId = userId;
            MessengerAccount = messengerAccount;
            IsShowCaptcha = isShowCaptcha;
        }

        public int UserId { get; }

        public string MessengerAccount { get; }

        public bool IsShowCaptcha { get; }
    }
}