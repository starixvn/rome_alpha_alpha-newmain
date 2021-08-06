namespace Alpha.Main.Site.ViewModels.Security
{
    using Alpha.Web.Models;
    using Helpers;

    public class SecurityCodeViewModel
    {
        public string SecurityCodeToken { get; set; }

        public string SecuritySalt { get; set; }

        public string HashKey { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel => Enumeration.FromValue<UserLevel>(CustRoleId).DisplayName.ToLower();

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public string Title { get; set; }

        public string UsernameShower { get; set; }

        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public bool IsEnabledBetLists { get; set; }

        public bool IsEnabledStatement { get; set; }
    }
}
