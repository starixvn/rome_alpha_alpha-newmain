namespace Alpha.Main.Site.App_Start
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Web.Mvc;
    using Fanex.Data.Repository;
    using Handlers;
    using MediatR;
    using Models.Menus;
    using Repositories;
    using Repositories.Impl;
    using Services;
    using Services.Impl;
    using SimpleInjector;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
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

            MenuItemConfig.Register(container);

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
            yield return typeof(MainHandler).Assembly;
        }

        private static void RegisterCrossCuttingConcerns(Container container)
        {
        }

        private static void RegisterServices(Container container)
        {
            container.Register<IDashboardService, DashboardService>(Lifestyle.Scoped);
            container.Register<IFileUploadService, FileUploadService>(Lifestyle.Scoped);
            container.Register<ISessionValidationService, SessionValidationService>(Lifestyle.Scoped);
            container.Register<IAnnouncementService, AnnouncementService>(Lifestyle.Scoped);
            container.Register<AppPathService, AppPathServiceImpl>(Lifestyle.Singleton);
            container.Register<SiteMenuService, SiteMenuServiceImpl>(Lifestyle.Scoped);
            container.Register<CachingService, CachingServiceImpl>(Lifestyle.Singleton);
            container.Register<MenuItemFactory, MenuItemFactoryImpl>(Lifestyle.Singleton);

        }

        private static void RegisterDataAccesses(Container container)
        {
            container.Register<IDynamicRepository, DynamicRepository>(Lifestyle.Scoped);
            container.Register<IDashboardRepository, DashboardRepository>(Lifestyle.Scoped);
        }
    }
}