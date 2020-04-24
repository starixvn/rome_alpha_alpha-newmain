namespace Alpha.Main.Site.ViewModels.Security
{
    public class ForceChangePasswordViewModel : PasswordViewModel
    {
        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public int ProfileImageId { get; set; }

        public string Username { get; set; }

        public string AlternateUsername { get; set; }

        public string UsernameShower { get; set; }

        public bool IsShowSkipForceChangePassword { get; set; }
    }
}