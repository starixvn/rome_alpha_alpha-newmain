namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using App_Start;
    using Fanex.Razorbill.Security.Configurations;
    using Fanex.Razorbill.Security.Sessions;
    using MediatR;
    using Models.Requests;
    using Models.Views;
    using Services;
    using Services.Impl;
    using Sunplus.Agent.Model.Entities;

    public class MenuBarHandler :
        IRequestHandler<MenuBarQuery, MenuBarViewModel>
    {
        private readonly SiteMenuService siteMenuService;      

        public MenuBarHandler(
            SiteMenuService siteMenuService)
        {
            this.siteMenuService = siteMenuService;            
        }

        public MenuBarViewModel Handle(MenuBarQuery message)
        {
            var menuItems = siteMenuService.GetMenuByUser(message.User);            

            var viewModel = new MenuBarViewModel(menuItems);

            return viewModel;
        }     
    }
}