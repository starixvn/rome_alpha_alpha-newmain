namespace Alpha.Main.Site.ViewModels.Security
{
    public class LoginSecurityCodeViewModel : BaseSecurityCodeViewModel
    {
        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public string Username { get; set; }

        public string AlternateUsername { get; set; }

        public string UsernameShower { get; set; }

        public LanguageViewModel Language { get; set; }
    }
}