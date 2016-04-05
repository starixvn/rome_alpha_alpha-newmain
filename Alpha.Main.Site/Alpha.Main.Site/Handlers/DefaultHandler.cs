namespace Alpha.Main.Site.Handlers
{
    using System;
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.ViewModels;
    using App_Start;
    using Configurations;
    using Helpers;
    using MediatR;
    using Sunplus.Agent.Model.Entities;

    public class DefaultHandler :
        IRequestHandler<EmptyQuery, IndexViewModel>,
        IRequestHandler<HeaderNavQuery, HeaderNavViewModel>
    {
        private readonly IUserSession userSession;
        private readonly string oldMainRootApp;

        internal DefaultHandler(IUserSession userSession, string oldMainRootApp)
        {
            this.userSession = userSession;
            this.oldMainRootApp = oldMainRootApp ?? AppPathHelper.GetAppPath(AppSettings.OldMainAppPath, "~/");
        }

        public DefaultHandler(IUserSession userSession) : this(userSession, null)
        {
        }

        public HeaderNavViewModel Handle(HeaderNavQuery message)
        {
            return new HeaderNavViewModel
            {
                HowToUseUrl = $"{this.oldMainRootApp}_HowToUse/Index.aspx",
                ContactUsUrl = $"{this.oldMainRootApp}Company/ContactUs/Index",
                ProfilePictureUrl = $"{this.oldMainRootApp}ContextualCustomer/ProfilePicture?custId={message.CustId}",
                ChangeSecurityCodeUrl = $"{this.oldMainRootApp}Security/SecurityCode/ChangeSecurityCode",
                ChangePasswordUrl = $"{this.oldMainRootApp}_MemberInfo/ChangePassword/ChangePassword.aspx"
            };
        }

        public IndexViewModel Handle(EmptyQuery message)
        {
            var viewModel = new IndexViewModel(userSession, FunctionRegistry.GetDefaultUrl);

            return viewModel;
        }
    }
}