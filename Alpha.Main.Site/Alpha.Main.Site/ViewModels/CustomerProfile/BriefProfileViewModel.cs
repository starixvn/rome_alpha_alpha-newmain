namespace Alpha.Main.Site.ViewModels.CustomerProfile
{
    using Main.Helpers;

    public class BriefProfileViewModel
    {
        public int CustId { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public string OldMainRootApp => MicroSites.OldMainPath("~/");

        public string Username { get; set; }

        public bool LoginUserIsNotMainAccount { get; set; }

        public string NickName { get; set; }

        public string IntroduceNickNameTooltipCookieName { get; set; }

        public bool IsEnableOtp { get; set; }
    }
}