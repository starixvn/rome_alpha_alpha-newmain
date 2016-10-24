using Alpha.Main.Helpers;
using Alpha.Main.Site.Helpers;

namespace Alpha.Main.Site.ViewModels.CustomerProfile
{
    public class BriefProfileViewModel
    {
        public int CustId { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public string OldMainRootApp
        {
            get
            {
                return MicroSites.OldMainUrl("~/");
            }
        }

        public string Username { get; set; }

        public bool LoginUserIsNotMainAccount { get; set; }

        public string NickName { get; set; }

        public string IntroduceNickNameTooltipCookieName { get; set; }

        public bool IsEnableOtp { get; set; }
    }
}