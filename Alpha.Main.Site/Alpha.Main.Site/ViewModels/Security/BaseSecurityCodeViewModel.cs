namespace Alpha.Main.Site.ViewModels.Security
{
    public class BaseSecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string PH { get; set; }

        public string RootUrl { get; set; }
    }
}