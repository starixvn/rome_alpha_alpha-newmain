namespace Alpha.Main.Site.App_Start
{
    using Alpha.Captcha.Services;
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.NetThrottling;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Resolvers;
    using Alpha.NetThrottling.Utils;
    using Chatbot.Client.Services;
    using DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using Fanex.Cryptography.Aes;
    using Fanex.Data.Repository;
    using Fanex.RemotingRef;
    using Handlers;
    using Main.Helpers;
    using Main.Helpers.Impl;
    using Main.Models;
    using Main.Models.Menus;
    using MediatR;
    using ONEbook.OTP.Helpers;
    using ONEbook.OTP.Services;
    using SecurityCode.Services;
    using Services;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using StackExchange.Redis;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using Web.Helpers;
    using Web.Helpers.Impl;

    public static class DependencyConfig
    {
        public static void Init()
        {
            RegisterProductConfiguration();

            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

            RegisterNetThrottling(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        private static void RegisterMediator(Container container)
        {
            var assemblies = GetAssemblies();
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), assemblies);
            container.Register(typeof(IAsyncRequestHandler<,>), assemblies);

            container.RegisterCollection(typeof(INotificationHandler<>), assemblies);
            container.RegisterCollection(typeof(IAsyncNotificationHandler<>), assemblies);
            container.RegisterSingleton(new SingleInstanceFactory(container.GetInstance));
            container.RegisterSingleton(new MultiInstanceFactory(container.GetAllInstances));
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            yield return typeof(DefaultHandler).Assembly;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Reviewed!")]
        private static void RegisterServices(Container container)
        {
            container.Register<IRemotingRefService, RemotingRefService>(Lifestyle.Singleton);
            container.Register<CachingManager, CachingManagerImpl>(Lifestyle.Singleton);
            container.Register<BrowserHelper, BrowserHelperImpl>(Lifestyle.Singleton);
            container.Register(() => Fanex.Logging.Logger.Log, Lifestyle.Singleton);

            container.RegisterPerWebRequest<IUserSession, UserSessionExt>();
            container.RegisterPerWebRequest<MenuItemBuilder, MenuItemBuilderImpl>();
            container.RegisterSingleton<CacheRefService, CacheRefServiceImpl>();

            container.RegisterPerWebRequest<SiteMenuService, SiteMenuServiceImpl>();
            container.Register<ProfilePictureServices, ProfilePictureServicesImpl>(Lifestyle.Scoped);
            container.RegisterPerWebRequest<SecurityCodeService, SecurityCodeServiceImpl>();
            container.RegisterPerWebRequest<SessionManager, SessionManagerImpl>();
            container.RegisterPerWebRequest<PasswordService, PasswordServiceImpl>();
            container.RegisterPerWebRequest<SiteMapReader, SiteMapReaderImpl>();
            container.RegisterPerWebRequest<Encryptor, EncryptorImpl>();
            container.RegisterPerWebRequest<BalanceUpDayInfoService, BalanceUpDayInfoServiceImpl>();
            container.RegisterPerWebRequest<BalanceInfoService, BalanceInfoServiceImpl>();
            container.RegisterPerWebRequest<StatisticsService, StatisticsServiceImpl>();
            container.RegisterPerWebRequest<SecurityCodeChecker, SecurityCodeCheckerImpl>();

            container.RegisterPerWebRequest<OtpService, OtpServiceImpl>();
            container.RegisterPerWebRequest<OtpAdapter, OtpAdapterImpl>();
            container.RegisterPerWebRequest<QrCodeAdapter, QrCodeAdapterImpl>();
            container.RegisterPerWebRequest<RandomHelper, RandomHelperImpl>();
            container.RegisterSingleton<JsonHelper, JsonHelperImpl>();
            container.RegisterSingleton<ExceptionHandlerWrapper, ExceptionHandlerWrapperImpl>();
            container.RegisterPerWebRequest<CustomerOtpService>(() =>
            new CustomerOtpServiceImpl(
                container.GetInstance<IDynamicRepository>(),
                container.GetInstance<OtpService>(),
                container.GetInstance<JsonHelper>(),
                container.GetInstance<INetThrottlingService>()));

            container.RegisterPerWebRequest<SettingService, SettingServiceImpl>();

            container.RegisterPerWebRequest<IUser, User>();
            container.RegisterSingleton(() => AesService.Default);

            container.RegisterPerWebRequest<LogoutService, LogoutServiceImpl>();

            container.RegisterPerWebRequest<NicknameService, NicknameServiceImpl>();

            container.RegisterPerWebRequest<NicknameChecker>(() =>
                new NicknameCheckerImpl(container.GetInstance<IUserSession>()));

            container.RegisterPerWebRequest<KickoutCustomerService, KickoutCustomerServiceImpl>();
            container.RegisterPerWebRequest<BannerService, BannerServiceImpl>();
            container.RegisterPerWebRequest<UMMessageService, UMMessageServiceImpl>();
            container.RegisterPerWebRequest<ISessionService, SessionService>();
            container.RegisterPerWebRequest<ProtectionCodeService, ProtectionCodeServiceImpl>();
            container.RegisterPerWebRequest<ThemeService, ThemeServiceImpl>();
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterPerWebRequest<IDynamicRepository, DynamicRepository>();
            container.RegisterPerWebRequest<SecurityCodeDataAccess, SecurityCodeDataAccessImpl>();
            container.RegisterPerWebRequest<PasswordDataAccess, PasswordDataAccessImpl>();
            container.RegisterPerWebRequest<StatisticsDataAccess, StatisticsDataAccessImpl>();
        }

        private static void RegisterProductConfiguration()
        {
            SimpleServiceLocator
              .Instance
              .Register<Func<int>>(AppSettingsHelper.GetInsertProfilePictureSiteMode);
        }

        private static void RegisterNetThrottling(Container container)
        {
            container.Register<INetThrottler, NetThrottler>(Lifestyle.Scoped);
            container.Register<IThrottleSetting, ThrottleSetting>(Lifestyle.Scoped);
            container.Register<IClock, Clock>(Lifestyle.Scoped);
            container.Register<ILogger, NetThrottling.Utils.Logger>(Lifestyle.Scoped);
            container.Register<ICacheServiceFactory, CacheServiceFactory>(Lifestyle.Scoped);
            container.Register<IResolver, BaseResolver>(Lifestyle.Scoped);
            container.Register<INetThrottlingService, NetThrottlingService>(Lifestyle.Scoped);

            container.RegisterSingleton<ICache>(() => CreateRedisCache());
            container.RegisterSingleton<ICacheService, CacheService>();
        }

        private static RedisCache CreateRedisCache()
        {
            var redisConfigurationString = ConfigurationManager.AppSettings["RedisConfigurationOptions:Alpha.NetThrottling"];
            var configurationOptions = ConfigurationOptions.Parse(redisConfigurationString);
            var redisCacheOptions = new RedisCacheOptions
            {
                ApplicationName = configurationOptions.ClientName,
                ConnectionOptions = configurationOptions
            };

            return new RedisCache(redisCacheOptions);
        }
    }
}