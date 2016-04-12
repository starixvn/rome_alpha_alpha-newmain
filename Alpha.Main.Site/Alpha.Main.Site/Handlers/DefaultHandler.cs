namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.ViewModels;
    using MediatR;
    using Services;

    public class DefaultHandler : IRequestHandler<IndexQuery, IndexViewModel>
    {
        /// <summary>
        /// Please do not change this value, because it was shared with old main site.
        /// </summary>
        private const string PasswordHashSessionKey = "ph";

        private readonly SessionManager sessionManager;

        public DefaultHandler(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var viewModel = new IndexViewModel(
                message.UserId,
                message.OnUserId,
                message.RoleId,
                message.Language,
                message.LanguageKey,
                FunctionRegistry.GetDefaultUrl);

            this.sessionManager.Add<string>(PasswordHashSessionKey, viewModel.PasswordHash);

            return viewModel;
        }
    }
}