namespace Alpha.Main.Site.ViewModels.Security
{
    public class BaseSecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string HashKey { get; set; }

        public string AppPath { get; set; }
    }
}