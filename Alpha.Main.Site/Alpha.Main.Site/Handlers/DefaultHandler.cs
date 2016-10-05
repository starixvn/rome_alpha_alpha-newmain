namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.Site.Models.Requests;
    using Alpha.Main.Site.ViewModels;
    using Helpers;
    using Main.Models.Enumerations;
    using MediatR;
    using Requests;
    using Services;

    public class DefaultHandler :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<BrowserQuery, BrowserViewModel>
    {
        /// <summary>
        /// Please do not change this value, because it was shared with old main site.
        /// </summary>
        private const string PasswordHashSessionKey = "ph";

        private const string SawBrowserWarningPageSessionKey = "DefaultHandler.SawBrowserWarningPageSessionKey";

        private readonly SessionManager sessionManager;
        private readonly BrowserHelper browserHelper;

        public DefaultHandler(SessionManager sessionManager, BrowserHelper browserHelper)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
        }

        public BrowserViewModel Handle(BrowserQuery message)
        {
            if (this.sessionManager.Get<bool>(SawBrowserWarningPageSessionKey))
            {
                return new BrowserViewModel();
            }

            var browser = browserHelper.CheckSupportedBrowsers();
            browser.UserLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            this.sessionManager.Add(SawBrowserWarningPageSessionKey, true);

            return browser;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var viewModel = new IndexViewModel(message, FunctionRegistry.GetDefaultUrl);

            this.sessionManager.Add<string>(PasswordHashSessionKey, viewModel.PasswordHash);

            return viewModel;
        }
    }
}