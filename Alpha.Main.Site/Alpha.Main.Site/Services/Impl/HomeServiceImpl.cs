namespace Alpha.Main.Site.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using Models;
    using Models.Enumerations;
    using Models.Menus;
    using Models.Requests.Home;
    using Sunplus.Agent.Model.Entities;

    public class HomeServiceImpl : HomeService
    {
        private readonly HomeDataAccess homeDataAccess;

        private readonly SiteMenuService siteMenuService;

        public HomeServiceImpl(HomeDataAccess homeDataAccess, SiteMenuService siteMenuService)
        {
            this.homeDataAccess = homeDataAccess;
            this.siteMenuService = siteMenuService;
        }

        public List<IMenuItem> GetNavigation(NavigationQuery query)
        {
            var siteNode = homeDataAccess.GetSiteMapNode();
            var menuList = new List<IMenuItem>();
            if (!siteNode.HasChildNodes)
            {
                return menuList;
            }

            var pageAccess = query.SubPageAccess;
            var isSubAcc = query.IsSubAcc;
            var subMenuCount = siteNode.ChildNodes.Count;

            var user = new User();
            user.Level = Enumeration.FromValue<UserLevel>(query.RoleId);
            user.SubAccountId = UserSession.Current.SubAccId;
            menuList = siteMenuService.GetMenuByUser(user).ToList();

            return menuList;
        }
    }
}