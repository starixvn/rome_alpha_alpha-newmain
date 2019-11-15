namespace Alpha.Main.Site.Handlers
{
    using Alpha.ProfilePicture.Services;
    using Fanex.Logging.Extensions.Web;
    using Helpers;
    using Main.Models.Enumerations;
    using MediatR;
    using Models.Requests;
    using Params;
    using Requests;
    using Services;
    using Sunplus.Agent.Utilities;
    using System;
    using ViewModels;
    using Web.Helpers;

    public class DefaultHandler :
        IRequestHandler<IndexQuery, IndexViewModel>,
        IRequestHandler<BrowserQuery, BrowserViewModel>,
        IRequestHandler<KickoutQuery, bool>
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
        private readonly ProfilePictureServices customerProfileService;
        private readonly LogoutService logoutService;

        public DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            ProfilePictureServices customerProfileService,
            LogoutService logoutService)
            : this(sessionManager, browserHelper, logoutService, null, null, customerProfileService)
        {
        }

        internal DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            LogoutService logoutService,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction,
            ProfilePictureServices customerProfileService)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
            this.logoutService = logoutService;

            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction ?? Encryption.PearsonEncode;
            this.customerProfileService = customerProfileService;
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
            var profile = customerProfileService.GetActorProfilePicture(message.CustId);

            var viewModel = new IndexViewModel(message, getDefaultUrl, encodeDataFunction, browserHelper, profile);

            sessionManager.Add(PasswordHashSessionKey, viewModel.PasswordHash);

            return viewModel;
        }

        public bool Handle(KickoutQuery message)
        {
            try
            {
                logoutService.Logout(new LogoutParam(
                    message.OnlineUserId,
                    message.ActorName,
                    message.SubUserName,
                    message.NickName ?? string.Empty,
                    message.ActorLevelId,
                    message.ServerId,
                    " Kicked out: Session Changed"));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
                return false;
            }

            return true;
        }
    }
}