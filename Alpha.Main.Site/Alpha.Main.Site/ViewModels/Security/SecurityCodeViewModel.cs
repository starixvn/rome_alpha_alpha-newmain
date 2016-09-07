namespace Alpha.Main.Site.ViewModels.Security
{
    using Alpha.Main.Models.Enumerations;

    public class SecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string Language { get; set; }

        public string VirtualDirectory { get; set; }
    }
}