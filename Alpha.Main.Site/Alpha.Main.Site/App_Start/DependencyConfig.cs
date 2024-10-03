namespace Alpha.Main.Site.App_Start
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using System.Threading;
    using System.Web.Mvc;
    using Alpha.Main.Configurations;
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.NetThrottling;
    using Alpha.NetThrottling.Models;
    using Alpha.NetThrottling.Resolvers;
    using Alpha.NetThrottling.Utils;
    using Alpha.SecurityCode.Services;
    using Alpha.Web.Helpers;
    using Alpha.Web.Helpers.Impl;
    using AlphaPlus.User.Models;
    using Fanex.Caching;
    using Fanex.Caching.Redis;
    using Fanex.Cryptography.Aes;
    using Fanex.Customer.Client.Configurations;
    using Fanex.Customer.Client.NETFramework;
    using Fanex.Customer.Client.Services;
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
    using Services;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Utilities;
    using TelegramBot.Client.Services;

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

            UserSession.Init(new HttpContextHelper());
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
            container.RegisterPerWebRequest<ApiKeyService>(() => new ApiKeyServiceImpl(CreateApiKeyHttpClient()));

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
                container.GetInstance<INetThrottlingService>(),
                container.GetInstance<IOTPService>()));

            container.RegisterPerWebRequest<ISettingService>(() => new SettingService(CreateTelegramBotHttpClient()));
            container.RegisterPerWebRequest<IBotService>(() => new BotService(CreateTelegramBotHttpClient()));
            container.RegisterPerWebRequest<INotificationService>(() => new NotificationService(CreateTelegramBotHttpClient()));
            container.RegisterPerWebRequest<IOTPService>(() => new OTPService(CreateTelegramBotHttpClient()));
            container.RegisterPerWebRequest<TelegramBotService, TelegramBotServiceImpl>();
            container.RegisterPerWebRequest<TransferService, TransferServiceImpl>();
            container.RegisterPerWebRequest<IUser, User>();
            container.RegisterSingleton(() => AesService.Default);

            container.RegisterPerWebRequest<LogoutService, LogoutServiceImpl>();

            container.RegisterPerWebRequest<NicknameService, NicknameServiceImpl>();

            container.RegisterPerWebRequest<NicknameChecker>(() =>
                new NicknameCheckerImpl(container.GetInstance<IUserSession>()));

            container.RegisterPerWebRequest<KickoutCustomerService, KickoutCustomerServiceImpl>();
            container.RegisterPerWebRequest<BannerService, BannerServiceImpl>();
            container.RegisterPerWebRequest<UMMessageService, UMMessageServiceImpl>();
            container.RegisterPerWebRequest<ProtectionCodeService, ProtectionCodeServiceImpl>();
            container.RegisterPerWebRequest<ThemeService, ThemeServiceImpl>();
            container.RegisterPerWebRequest<UpdateUsageLanguageService, UpdateUsageLanguageServiceImpl>();
            container.RegisterPerWebRequest<WhiteLabelService, WhiteLabelServiceImpl>();
            container.RegisterSingleton(new CustomerClientConfiguration().Initialize());
            container.RegisterSingleton<ICustomerService, Fanex.Customer.Client.Services.CustomerService>();
            container.RegisterPerWebRequest<Services.CustomerService, CustomerServiceImpl>();
            container.RegisterPerWebRequest<INovaCasinoService, NovaCasinoService>();
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterPerWebRequest<IDynamicRepository, DynamicRepository>();
        }

        private static void RegisterProductConfiguration()
        {
            SimpleServiceLocator
              .Instance
              .Register<Func<int>>(AppSettingsHelper.GetInsertProfilePictureSiteMode);
        }

        private static void RegisterNetThrottling(Container container)
        {
            container.RegisterPerWebRequest<INetThrottler>(() => new NetThrottler(isUsedRedis: false));
            container.RegisterPerWebRequest<IThrottleSetting>(() => new ThrottleSetting(isUsedRedis: false));
            container.Register<IClock, Clock>(Lifestyle.Scoped);
            container.Register<ILogger, Logger>(Lifestyle.Scoped);
            container.Register<ICacheServiceFactory, CacheServiceFactory>(Lifestyle.Scoped);
            container.Register<IResolver, BaseResolver>(Lifestyle.Scoped);
            container.Register<INetThrottlingService, NetThrottlingService>(Lifestyle.Scoped);

            container.RegisterSingleton<ICache>(() => CreateRedisCache());
            container.RegisterSingleton<ICacheService, CacheService>();
        }

        private static RedisCache CreateRedisCache()
        {
            var redisConfigurationString = ConfigurationManager.AppSettings["RedisConfigurationOptions:Alpha.NetThrottling"];
            var redisCacheOptions = new RedisCacheOptions
            {
                ConnectionString = redisConfigurationString
            };

            return new RedisCache(redisCacheOptions);
        }

        private static HttpClient CreateTelegramBotHttpClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var telegramClient = HttpClientFactory.Create();
            telegramClient.BaseAddress = AppSettings.TelegramBotClientUri;
            telegramClient.DefaultRequestHeaders.Add("Accept-Language", Thread.CurrentThread.CurrentCulture.Name);

            return telegramClient;
        }

        private static HttpClient CreateApiKeyHttpClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var apiKeyClient = HttpClientFactory.Create();
            apiKeyClient.BaseAddress = AppSettings.ApiKeyServiceUri;
            apiKeyClient.DefaultRequestHeaders.Add("Accept-Language", Thread.CurrentThread.CurrentCulture.Name);

            return apiKeyClient;
        }
    }
}