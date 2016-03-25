namespace Alpha.Main.Site.Models.Views.CustomerProfile
{
    public class BriefProfileViewModel
    {
        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public int CountNicknameTooltip { get; set; }

        public bool IsShowEditProfilePictureTooltip { get; set; }

        public string OldMainPath { get; set; }

        public string ChangeNickNameUrl { get; set; }

        public string ViewProfileUrl { get; set; }

        public string Username { get; set; }

        public bool IsShowConfigUrl { get; set; }

        public bool IsNotMainAccount { get; set; }

        public string SubAccUserName { get; set; }

        public string AdminName { get; set; }

        public string NickName { get; set; }

        public bool IsShowIntroduceNickName { get; set; }

        public string MainFrameName
        {
            get
            {
                return "main";
            }
        }
    }
}