namespace Alpha.Main.Site.Handlers
{
    using System;
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

        private readonly SessionManager sessionManager;

        public DefaultHandler(SessionManager sessionManager)
        {
            this.sessionManager = sessionManager;
        }

        public BrowserViewModel Handle(BrowserQuery message)
        {
            var browser = BrowserHelper.CheckSupportedBrowsers();
            browser.UserLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            return browser;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var viewModel = new IndexViewModel(message, FunctionRegistry.GetDefaultUrl, FunctionRegistry.ShowIntroduction);

            this.sessionManager.Add<string>(PasswordHashSessionKey, viewModel.PasswordHash);

            return viewModel;
        }
    }
}