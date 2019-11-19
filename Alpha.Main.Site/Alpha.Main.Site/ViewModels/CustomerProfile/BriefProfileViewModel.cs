namespace Alpha.Main.Site.ViewModels.CustomerProfile
{
    using Helpers;

    public class BriefProfileViewModel
    {
        public int CustId { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public string OldMainRootApp => UrlHelper.MapOldMainPath("~/");

        public string AccountInfoRootPath => UrlHelper.MapAccountInfoPath("~/");

        public string Username { get; set; }

        public bool IsInternal { get; set; }

        public string NickName { get; set; }

        public string IntroduceNickNameTooltipCookieName { get; set; }

        public bool IsEnableOtp { get; set; }

        public bool IsHideNicknameLink { get; set; }

        public string SubAccountTooltip { get; set; }

        public bool IsShowProfilePictureTooltip { get; set; }
    }
}
