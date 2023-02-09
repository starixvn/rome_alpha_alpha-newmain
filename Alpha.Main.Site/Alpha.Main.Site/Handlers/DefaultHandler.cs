namespace Alpha.Main.Site.Handlers
{
    using Alpha.Main.ProfilePicture.Services;
    using Alpha.SecurityCode.Web;
    using Alpha.Web.Models;
    using Fanex.Logging.Extensions.Web;
    using MediatR;
    using Models.Requests;
    using Params;
    using Requests;
    using Services;
    using Sunplus.Agent.Utilities;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TelegramBot.Client.Models;
    using TelegramBot.Client.Services;
    using TelegramBot.Constants;
    using ViewModels;
    using Web.Helpers;

    public class DefaultHandler :
        IAsyncRequestHandler<IndexQuery, IndexViewModel>,
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
        private readonly ISettingService telegramSettingService;
        private readonly FeedbackService feedbackService;

        public DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            ProfilePictureServices customerProfileService,
            LogoutService logoutService,
            ISettingService telegramSettingService,
            FeedbackService feedbackService)
            : this(sessionManager, browserHelper, logoutService, null, null, customerProfileService, telegramSettingService, feedbackService)
        {
        }

        internal DefaultHandler(
            SessionManager sessionManager,
            BrowserHelper browserHelper,
            LogoutService logoutService,
            Func<string> getDefaultUrl,
            Func<string, string> encodeDataFunction,
            ProfilePictureServices customerProfileService,
            ISettingService telegramSettingService,
            FeedbackService feedbackService)
        {
            this.sessionManager = sessionManager;
            this.browserHelper = browserHelper;
            this.logoutService = logoutService;

            this.getDefaultUrl = getDefaultUrl ?? FunctionRegistry.GetDefaultUrl;
            this.encodeDataFunction = encodeDataFunction ?? Encryption.PearsonEncode;
            this.customerProfileService = customerProfileService;
            this.telegramSettingService = telegramSettingService;
            this.feedbackService = feedbackService;
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

        public async Task<IndexViewModel> Handle(IndexQuery message)
        {
            var profile = customerProfileService.GetActorProfilePicture(message.CustId);

            var viewModel = new IndexViewModel(message, getDefaultUrl, encodeDataFunction, browserHelper, profile);

            var custBotMessenger = await GetCustBotMessenger(message.CustId, message.SubAccId, message.Language);
            if (custBotMessenger == null)
            {
                viewModel.IsShowSuggestEnableTelegramOtp = false;
            }
            else
            {
                var otpFuntion = custBotMessenger.Functions.FirstOrDefault(func => func.FunctionId == FunctionIds.OTP);
                if (!otpFuntion.Enabled && SecurityCodeSessionManager.SecurityCodeSession.IsEnabledOtp)
                {
                    viewModel.IsShowSuggestEnableTelegramOtp = true;
                }
            }

            sessionManager.Add(PasswordHashSessionKey, viewModel.PasswordHash);

            viewModel.ShowFloatingFeedback = !(await feedbackService.CheckExistsAsync(message.CustId));

            return viewModel;
        }

        private async Task<CustBotMessenger> GetCustBotMessenger(int custId, int subAccId, string language)
        {
            try
            {
                return await telegramSettingService.GetCustBotMessengerAsync(custId, subAccId, language);
            }
            catch (Exception ex)
            {
                await Fanex.Logging.Logger.Log.ErrorAsync(ex.Message, ex);
                return null;
            }

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