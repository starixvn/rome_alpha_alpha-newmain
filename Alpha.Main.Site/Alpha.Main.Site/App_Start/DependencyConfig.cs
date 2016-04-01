namespace Alpha.Main.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess;
    using DataAccess.Impl;
    using Fanex.Data.Repository;
    using Handlers;
    using MediatR;
    using Models.Menus;
    using Services;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Sunplus.Agent.Model.Entities;

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
            yield return typeof(HomeHandler).Assembly;
        }

        private static void RegisterCrossCuttingConcerns(Container container)
        {
        }

        private static void RegisterServices(Container container)
        {
            container.Register<IUserSession>(
                () =>
                {
                    if (HttpContext.Current != null
                        && (HttpContext.Current.Handler is IRequiresSessionState
                        || HttpContext.Current.Handler is IReadOnlySessionState))
                    {
                        return UserSession.Current;
                    }

                    return new UserSessionBase(string.Empty);
                },
                Lifestyle.Transient);

            container.Register<IDashboardService, DashboardService>(Lifestyle.Transient);
            container.Register<IAnnouncementService, AnnouncementService>(Lifestyle.Transient);
            container.Register<AppPathService, AppPathServiceImpl>(Lifestyle.Singleton);
            container.Register<SiteMenuService, SiteMenuServiceImpl>(Lifestyle.Transient);
            container.Register<CachingService, CachingServiceImpl>(Lifestyle.Singleton);
            container.Register<MenuItemBuilder, MenuItemBuilderImpl>(Lifestyle.Singleton);
            container.Register<CustomerProfileService, CustomerProfileServiceImpl>(Lifestyle.Transient);
            container.Register<SecurityCodeService, SecurityCodeServiceImpl>(Lifestyle.Transient);
            container.Register<UserPermissionService, UserPermissionServiceImpl>(Lifestyle.Transient);
            container.Register<UserMessageService, UserMessageServiceImpl>(Lifestyle.Transient);
        }

        private static void RegisterDataAccesses(Container container)
        {
            container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Transient);
            container.Register<CustomerProfileDataAccess, CustomerProfileDataAccessImpl>(Lifestyle.Transient);
        }
    }
}