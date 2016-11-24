namespace Alpha.Main.Site.ViewModels.Security
{
    using Alpha.Main.AppResources;
    using Alpha.Main.Site.Requests.Otp;
    using Main.Models;

    public class ResetOtpViewModel : ResetOtpQuery
    {
        public string ResetTitle
        {
            get
            {
                return string.Format(Security.RemoveOtpForCust, Username);
            }
        }

        public Result Result { get; set; }
    }
}