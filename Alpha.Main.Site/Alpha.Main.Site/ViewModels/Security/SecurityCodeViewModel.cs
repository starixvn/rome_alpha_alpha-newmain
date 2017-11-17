namespace Alpha.Main.Site.ViewModels.Security
{
    using Helpers;
    using Main.Models.Enumerations;

    public class SecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string HashKey { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(CustRoleId).DisplayName.ToLower();
            }
        }

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public string Title { get; set; }
    }
}
