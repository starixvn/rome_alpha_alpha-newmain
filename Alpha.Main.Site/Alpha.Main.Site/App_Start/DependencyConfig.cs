#pragma warning disable 618, 612

namespace Alpha.Main.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web.Mvc;
    using DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Data.Repository;
    using Handlers;
    using Helpers;
    using Main.Helpers;
    using Main.Helpers.Impl;
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
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Casino;
    using Sunplus.Agent.Model.Entities;
    using Sunplus.Agent.Utilities;
    using Web.Helpers;
    using Web.Helpers.Impl;

    [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Reviewed!")]
    public static class DependencyConfig
    {
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        public static void Init()
        {
            var container = new Container();

            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            RegisterMediator(container);

            RegisterCrossCuttingConcerns(container);

            RegisterDataAccesses(container);

            RegisterServices(container);

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

        private static void RegisterCrossCuttingConcerns(Container container)
        {
            container.Register<IDateTime, DateTimeWrap>(Lifestyle.Singleton);
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed!")]
        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Reviewed!")]
        private static void RegisterServices(Container container)
        {
            container.Register<CachingManager, CachingManagerImpl>(Lifestyle.Singleton);
            container.Register<BrowserHelper, BrowserHelperImpl>(Lifestyle.Singleton);

            container.RegisterPerWebRequest<IUserSession, UserSessionExt>();
            container.RegisterPerWebRequest<MenuItemBuilder, MenuItemBuilderImpl>();

            container.RegisterPerWebRequest<SiteMenuService, SiteMenuServiceImpl>();
            container.RegisterPerWebRequest<CustomerProfileService, CustomerProfileServiceImpl>();
            container.RegisterPerWebRequest<SecurityCodeService, SecurityCodeServiceImpl>();
            container.RegisterPerWebRequest<SessionManager, SessionManagerImpl>();
            container.RegisterPerWebRequest<PasswordService, PasswordServiceImpl>();
            container.RegisterPerWebRequest<SiteMapReader, SiteMapReaderImpl>();
            container.RegisterPerWebRequest<Encryptor, EncryptorImpl>();
            container.RegisterPerWebRequest<BalanceUpDayInfoService, BalanceUpDayInfoServiceImpl>();
            container.RegisterPerWebRequest<BalanceInfoService, BalanceInfoServiceImpl>();
            container.RegisterPerWebRequest<StatisticsService, StatisticsServiceImpl>();
            container.RegisterPerWebRequest<CasinoService, CasinoServiceImpl>();
            container.RegisterPerWebRequest<SecurityCodeChecker, SecurityCodeCheckerImpl>();
            container.RegisterPerWebRequest<MemberRequestsService, MemberRequestsServiceImpl>();

            container.RegisterPerWebRequest<OtpService, OtpServiceImpl>();
            container.RegisterPerWebRequest<OtpAdapter, OtpAdapterImpl>();
            container.RegisterPerWebRequest<QrCodeAdapter, QrCodeAdapterImpl>();
            container.RegisterPerWebRequest<RandomHelper, RandomHelperImpl>();
            container.RegisterPerWebRequest<JsonHelper, JsonHelperImpl>();
            container.RegisterPerWebRequest<CustomerOtpService>(() =>
            new CustomerOtpServiceImpl(
                container.GetInstance<IDynamicRepository>(),
                container.GetInstance<OtpService>(),
                container.GetInstance<JsonHelper>(),
                container.GetInstance<CasinoService>()));
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterPerWebRequest<IDynamicRepository, DynamicRepository>();
            container.RegisterPerWebRequest<CustomerProfileDataAccess, CustomerProfileDataAccessImpl>();
            container.RegisterPerWebRequest<SecurityCodeDataAccess, SecurityCodeDataAccessImpl>();
            container.RegisterPerWebRequest<PasswordDataAccess, PasswordDataAccessImpl>();
            container.RegisterPerWebRequest<StatisticsDataAccess, StatisticsDataAccessImpl>();
        }
    }
}