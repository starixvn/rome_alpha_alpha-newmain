namespace Alpha.Main.Site.ViewModels.Security
{
    using Alpha.Main.Models.Enumerations;

    public class SecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string PH { get; set; }

        public string Avatar { get; set; }

        public string NickName { get; set; }

        public string Language { get; set; }
    }
}