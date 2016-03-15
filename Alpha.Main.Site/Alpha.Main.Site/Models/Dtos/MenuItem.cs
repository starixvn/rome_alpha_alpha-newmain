namespace Alpha.Main.Site.Models.Dtos
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using App_GlobalResources;

    public class MenuItem
    {
        private static readonly MenuItem Reports = new MenuItem("reports", "[E]");
        private static readonly MenuItem TotalBets = new MenuItem("totalbets", "[D]");
        private static readonly MenuItem MemberInfo = new MenuItem("memberInfo", "[H]");
        private static readonly MenuItem Transfer = new MenuItem("transfer", "[C]");
        private static readonly MenuItem ViewLog = new MenuItem("viewLog", "[G]");

        public MenuItem()
        {
        }

        public MenuItem(string key)
        {
            Key = key;
        }

        private MenuItem(string key, string functionType)
        {
            Key = key;
            Function = functionType;
        }

        public string Key { get; set; }

        public string Name { get; set; }

        public string DisplayName
        {
            get
            {
                return Alpha_sitemap.ResourceManager.GetString(this.Key);
            }
        }

        public string Url { get; set; }

        public string Function { get; set; }

        public List<MenuItem> SubMenus { get; set; }

        public bool ShowSubMenus { get; set; }

        public bool IsAccessibleToUser(IUser user)
        {
            if (user == null || user.PageAccess == null)
            {
                throw new ArgumentNullException(nameof(IsAccessibleToUser));
            }

            var canView = user.IsMainAccount || 
              (ThisMenuIs(Reports) && user.HasPermissionOnFunction(Reports.Function)) ||
              (ThisMenuIs(TotalBets) && user.HasPermissionOnFunction(TotalBets.Function)) ||
              (ThisMenuIs(MemberInfo) && user.HasPermissionOnFunction(MemberInfo.Function)) ||
              (ThisMenuIs(Transfer) && user.HasPermissionOnFunction(Transfer.Function)) ||
              (ThisMenuIs(ViewLog) && user.HasPermissionOnFunction(ViewLog.Function));

            return canView;
        }

        private bool ThisMenuIs(MenuItem item)
        {
            return this.Key == item.Key;
        }
    }
}