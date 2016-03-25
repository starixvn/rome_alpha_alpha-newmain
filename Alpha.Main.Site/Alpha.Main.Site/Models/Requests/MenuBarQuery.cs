namespace Alpha.Main.Site.Models.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels;
    using Views;

    public class MenuBarQuery : IRequest<MenuBarViewModel>
    {
        public IUser User
        {
            get
            {
                var user = new User();
                user.Level = Enumeration.FromValue<UserLevel>(UserSession.Current.RoleId);
                return user;
            }
        }
    }
}