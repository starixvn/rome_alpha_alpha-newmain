namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.ViewModels;
    using App_Start;
    using MediatR;

    public class DefaultHandler :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<HeaderNavQuery, HeaderNavViewModel>

    {
        public IndexViewModel Handle(IndexQuery message)
        {
            var viewModel = new IndexViewModel(
                message.UserId,
                message.OnUserId,
                message.RoleId,
                message.Language,
                message.LanguageKey,
                FunctionRegistry.GetDefaultUrl);

            return viewModel;
        }


        public HeaderNavViewModel Handle(HeaderNavQuery message)
        {
            return new HeaderNavViewModel
            {
                //HowToUseUrl = $"{this.oldMainRootApp}_HowToUse/Index.aspx",
                //ContactUsUrl = $"{this.oldMainRootApp}Company/ContactUs/Index",
                //ProfilePictureUrl = $"{this.oldMainRootApp}ContextualCustomer/ProfilePicture?custId={message.CustId}",
                //ChangeSecurityCodeUrl = $"{this.oldMainRootApp}Security/SecurityCode/ChangeSecurityCode",
                //ChangePasswordUrl = $"{this.oldMainRootApp}_MemberInfo/ChangePassword/ChangePassword.aspx"
            };
        }
    }
}