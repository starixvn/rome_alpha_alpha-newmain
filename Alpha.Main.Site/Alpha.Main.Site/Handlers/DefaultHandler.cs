namespace Alpha.Main.Site.Handlers
{
    using System;
    using Helpers;
    using Main.Models.Enumerations;
    using MediatR;
    using Models.Requests;
    using Requests;
    using Services;
    using Sunplus.Agent.Utilities;
    using ViewModels;

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
        private readonly Func<string> getDefaultUrl;
        private readonly Func<string, string> encodeDataFunction;

        public DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper)
            : this(sessionManager, browserHelper, null, null)
        {

        }

        internal DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;

            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction ?? Encryption.PearsonEncode;
        }

        public BrowserViewModel Handle(BrowserQuery message)
        {
            if (sessionManager.Get<bool>(SawBrowserWarningPageSessionKey))
            {
                return new BrowserViewModel();
            }

            var browser = browserHelper.CheckSupportedBrowsers();
            browser.UserLevel = Enumeration.FromValue<UserLevel>(message.UserLevelId);

            sessionManager.Add(SawBrowserWarningPageSessionKey, true);

            return browser;
        }

        public IndexViewModel Handle(IndexQuery message)
        {
            var viewModel = new IndexViewModel(message, getDefaultUrl, encodeDataFunction, browserHelper);

            sessionManager.Add(PasswordHashSessionKey, viewModel.PasswordHash);

            return viewModel;
        }
    }
}
