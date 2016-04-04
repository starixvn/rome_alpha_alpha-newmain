namespace Alpha.Main.Site.Services.Impl
{
    using System.Web;
    using App_GlobalResources;
    using Fanex.Age.UI;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Models.Dtos;
    using Models.Requests;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.Entities;

    public class DashboardService 
    {
        private readonly SiteMenuService siteMenuService;

        private IUserSession userSession;

        public DashboardService(
            SiteMenuService siteMenuService,
            IUserSession userSession)
        {
            this.siteMenuService = siteMenuService;
            this.userSession = userSession;
        }

        public ResultModel ChangeLanguage(ChangeLanguageQuery message)
        {
            var result = new ResultModel();
            if (!Validation.CheckValidLanguage(message.Language))
            {
                result.ResultCode = 1;
                result.ResultMessage = Resources.strNotAccessPage;
            }
            else
            {
                UserSession.Current.Language = message.Language;
                UserSession.Current.LangKey = Language.LangKeyFromCulture(message.Language);
                AppSession.Language = message.Language;
            }

            return result;
        }

        //public void StoreSession(MainIndexViewModel dashboardViewModel)
        //{
        //    HttpContext.Current.Session["ph"] = dashboardViewModel.PasswordHash;

        //    // _sessionValidationService.AddUsernameTokenToCookie();
        //    // SessionValidator.AddSessionVerificationToken();
        //}
    }
}