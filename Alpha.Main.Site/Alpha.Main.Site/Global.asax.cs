namespace Alpha.Main.Site
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Alpha.Auth.Configurations;
    using Alpha.Auth.Filters;
    using Alpha.Web;
    using Alpha.Web.Filters;
    using Alpha.Web.Helpers;
    using AlphaPlus.Logging;
    using AlphaPlus.Logging.Web;
    using AlphaPlus.User.Models;
    using App_Start;
    using CaptchaMvc.Infrastructure;
    using CaptchaMvc.Interface;
    using Fanex.Caching;
    using Fanex.Data;
    using Fanex.Logging.Extensions.Web;
    using Fanex.RemotingRef;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            MvcHandler.DisableMvcResponseHeader = true;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var connectionStrings = ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .ToDictionary(
                    connection => connection.Name,
                    connection => new ConnectionConfiguration(connection.Name, connection.ConnectionString, connection.ProviderName),
                    StringComparer.OrdinalIgnoreCase);
            DbSettingProviderManager
                .StartNewSession()
                .Use(connectionStrings)
                .Use(Configurations.AppSettings.FanexDataServiceApi)
                .Register(resourcePath: Configurations.AppSettings.StoreProcedurePath)
                .Run();

            AlphaPlus.Configurations.ConfigurationManager.Init(new ConfigurationHelper());
            LoggingConfig.ExceptionHandler = new ExceptionHandlerAdapter();
            LoggingConfig.Init(new[] { "password", "seccode", "securitycode" });

            DependencyConfig.Init();
            StaticResources.Init(GetBuildVersion());
            ShareSession.Init();

            var umConfig = new UMClientConfiguration().Initialize();
            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new UMCacheService(new CacheService()))
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new ONEbook.UM.Helpers.HttpContextHelper());
            GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));

            GlobalFilters.Filters.Add(new CheckBelongingFilter());
            GlobalFilters.Filters.Add(new NoCacheFilter());
            GlobalFilters.Filters.Add(new AfterLoginFlowFilter());
            GlobalFilters.Filters.Add(new ProtectedModeFilter());

            AntiForgeryConfig.SuppressXFrameOptionsHeader = true;

            RefDataTypeContainer
                .ConnectTo(new Uri(Configurations.AppSettings.RemoteRefURL))
                .FromApplication("Alpha.Main.Site");
            CaptchaUtils.CaptchaManager.StorageProvider = new CookieStorageProvider();
            var defaultCaptchaManager = (DefaultCaptchaManager)CaptchaUtils.CaptchaManager;
            defaultCaptchaManager.ImageUrlFactory = (helper, pair) => ImageUrlFactory(defaultCaptchaManager, helper, pair);
            defaultCaptchaManager.RefreshUrlFactory = RefreshUrlFactory;

            if (Configurations.AppSettings.IsStandaloneReports)
            {
                Helpers.UrlHelper.MapReportsPath = Helpers.UrlHelper.MapReportsDomain;
            }
        }

        /// <summary>
        /// Application error event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        protected void Application_Error(object sender, EventArgs e)
        {
            if (Request.IsLocal)
            {
                return;
            }

            Exception ex = Server.GetLastError();
            var currentCustomer = UserSession.Current;

            if (ex is FormatException &&
                Request.Params.AllKeys.Any(key => AuthorizationSection.Instance.CustIdQueryStringParamNames.Any(cf => string.Equals(key, cf, StringComparison.OrdinalIgnoreCase))))
            {
                return;
            }

            ExceptionHandler.Handle(ex,
                new Dictionary<string, object>
                {
                    ["CustId"] = currentCustomer?.CustId,
                    ["CustName"] = currentCustomer?.UserName,
                    ["SubAccountName"] = currentCustomer?.SubUserName,
                    ["AdminName"] = currentCustomer?.AdminName
                });
        }

        private static string GetBuildVersion()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return $"{versionInfo.FileVersion}.{Configurations.AppSettings.JsVersion}";
        }

        private string RefreshUrlFactory(UrlHelper urlHelper, KeyValuePair<string, ICaptchaValue> keyValuePair)
        {
            return urlHelper.Action("Refresh", "DefaultCaptcha", new { area = string.Empty });
        }

        /// <summary>
        /// Image URL Factory.
        /// </summary>
        /// <param name="captchaManager">Captcha Manager.</param>
        /// <param name="urlHelper">URL Helper.</param>
        /// <param name="keyValuePair">Key Value pair.</param>
        /// <returns>Return Result.</returns>
        private static string ImageUrlFactory(DefaultCaptchaManager captchaManager, System.Web.Mvc.UrlHelper urlHelper, KeyValuePair<string, ICaptchaValue> keyValuePair)
        {
            return urlHelper.Action(
                "Generate",
                "DefaultCaptcha",
                new RouteValueDictionary
                                {
                                    {
                                        "area", string.Empty
                                    },
                                    {
                                        captchaManager.TokenParameterName, keyValuePair.Key
                                    }
                                });
        }
    }
}