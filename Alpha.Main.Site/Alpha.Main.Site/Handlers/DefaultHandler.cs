namespace Alpha.Main.Site.Handlers
{
    using System;
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.ViewModels;
    using App_Start;
    using Configurations;
    using Helpers;
    using MediatR;

    public class DefaultHandler :
        IRequestHandler<IndexQuery, IndexViewModel>
        IRequestHandler<HeaderNavQuery, HeaderNavViewModel>
        private readonly string oldMainRootApp;
       
    {
        public IndexViewModel Handle(IndexQuery message)
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
        {
            var viewModel = new IndexViewModel(
                message.RoleId,
                message.Language,
                message.LanguageKey,
                message.OnUserId,
                FunctionRegistry.GetDefaultUrl);

            return viewModel;
        }
    }
}