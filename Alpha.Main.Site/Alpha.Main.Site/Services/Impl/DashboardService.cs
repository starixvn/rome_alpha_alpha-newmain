namespace Alpha.Main.Site.Services.Impl
{
    using App_GlobalResources;
    using Fanex.Age.UI;
    using Fanex.Common;
    using Fanex.CustomerSetting.Module.Core.Web;
    using Models.Dtos;
    using Models.Requests;
    using Models.Views;
    using Sunplus.Agent.Model.Common;
    using Sunplus.Agent.Model.Entities;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web;
    using System.Xml;
    using SiteServices = Alpha.Main.Site.Services;
    using System.Linq;
    public class DashboardService : IDashboardService
    {
        private readonly SiteMenuService siteMenuService;

        private AppPathService _appPathService;
        private IUserSession _userSession;

        public DashboardService(
            AppPathService appPathService,
            SiteMenuService siteMenuService,
            IUserSession userSession)
        {
            _appPathService = appPathService;
            this.siteMenuService = siteMenuService;
            _userSession = userSession;
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

        public void StoreSession(MainIndexViewModel dashboardViewModel)
        {
            HttpContext.Current.Session["ph"] = dashboardViewModel.PasswordHash;

            // _sessionValidationService.AddUsernameTokenToCookie();
            // SessionValidator.AddSessionVerificationToken();
        }

        private string GetOldMainAppPath()
        {
            return _appPathService.GetAppPath(SiteServices.AppSettings.OldMainAppPath, "~/");
        }
    }
}