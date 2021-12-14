namespace Alpha.Main.Site.ViewModels.Security
{
    using Alpha.Web.Models;
    using Helpers;

    public class PasswordViewModel
    {
        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public byte CustRoleId { get; set; }

        public string UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(CustRoleId).DisplayName.ToLower();
            }
        }

        public string HashKey { get; set; }

        public bool EnableSecurityCode { get; set; }

        public string OldMainRootPath => UrlHelper.MapOldMainPath("~/");

        public string MainRootPath => UrlHelper.MapMainPath("~/");

        public bool IsChangeMemberPassword
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(CustRoleId).IsMemberLevel();
            }
        }

        public string Title { get; set; }
    }
}
