namespace Alpha.Main.Site.App_Start
{
    using DataAccesses;
    using DataAccesses.Impl;
    using Fanex.Data.Repository;
    using Handlers;
    using Main.Helpers;
    using Main.Helpers.Impl;
    using Main.Models.Menus;
    using MediatR;
    using Services;
    using Services.Impl;
    using Sessions;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Model.Base;
    using Sunplus.Agent.Model.Entities;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;

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
            container.RegisterPerWebRequest<IUserSession, UserSessionExt>();

            container.Register(
                () =>
                {
                    if (HttpContext.Current != null
                       && (HttpContext.Current.Handler is IRequiresSessionState
                       || HttpContext.Current.Handler is IReadOnlySessionState))
                    {
                        return CookieLessSessionFactory.SecurityCodeSession;
                    }

                    return new SecurityCodeSession(string.Empty);
                },
                Lifestyle.Transient);

            //// Singleton
            container.Register<CachingManager, CachingManagerImpl>(Lifestyle.Singleton);
            container.RegisterPerWebRequest<MenuItemBuilder, MenuItemBuilderImpl>();

            //// Transient
            container.RegisterPerWebRequest<AnnouncementService, AnnouncementServiceImpl>();
            container.RegisterPerWebRequest<SiteMenuService, SiteMenuServiceImpl>();
            container.RegisterPerWebRequest<CustomerProfileService, CustomerProfileServiceImpl>();
            container.RegisterPerWebRequest<SecurityCodeService, SecurityCodeServiceImpl>();
            container.RegisterPerWebRequest<UserMessageService, UserMessageServiceImpl>();
            container.RegisterPerWebRequest<SessionManager, SessionManagerImpl>();
            container.RegisterPerWebRequest<SiteMapReader, SiteMapReaderImpl>();
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.RegisterPerWebRequest<IDynamicRepository, DynamicRepository>();
            container.RegisterPerWebRequest<CustomerProfileDataAccess, CustomerProfileDataAccessImpl>();
            container.Register<SecurityCodeDataAccess, SecurityCodeDataAccessImpl>(Lifestyle.Transient);
        }
    }
}