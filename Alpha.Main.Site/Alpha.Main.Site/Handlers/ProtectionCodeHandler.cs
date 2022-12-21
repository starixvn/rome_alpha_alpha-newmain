namespace Alpha.Main.Site.Handlers
{
    using System.Threading.Tasks;
    using Alpha.Main.Params.ProtectionCode;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.ProtectionCode;
    using Alpha.Main.Site.ViewModels.ProtectionCode;
    using Alpha.SecurityCode.Services;
    using Alpha.Web.Helpers;
    using Alpha.Web.Models;
    using MediatR;
    using ResultModel = Main.Models.Result;

    public class ProtectionCodeHandler :
        IAsyncRequestHandler<ProtectionCodeQuery, ProtectionCodeViewModel>,
        IAsyncRequestHandler<UpdateProtectionCodeCommand, ResultModel>
    {
        private const string HashKeySessionKey = "Alpha.Main.ProtectionCodeHandler.ProtectionCodeHash";

        private readonly SessionManager sessionManager;
        private readonly NicknameChecker nicknameChecker;
        private readonly ProtectionCodeService protectionCodeService;
        private readonly SecurityCodeChecker securityCodeChecker;

        public ProtectionCodeHandler(
            SessionManager sessionManager,
            NicknameChecker nicknameChecker,
            ProtectionCodeService protectionCodeService,
            SecurityCodeChecker securityCodeChecker)
        {
            this.sessionManager = sessionManager;
            this.nicknameChecker = nicknameChecker;
            this.protectionCodeService = protectionCodeService;
            this.securityCodeChecker = securityCodeChecker;
        }

        public async Task<ProtectionCodeViewModel> Handle(ProtectionCodeQuery message)
        {
            var isShowNicknameLink = !nicknameChecker.IsOfferForceLoginAndOverDueDate;
            var protectionCode = await protectionCodeService.GetAsync(message.CustId, message.SubAccId);

            return new ProtectionCodeViewModel(
                message.CustId,
                Enumeration.FromValue<UserLevel>(message.RoleId).DisplayName.ToLowerInvariant(),
                message.Language,
                HashKey,
                isShowNicknameLink,
                !string.IsNullOrWhiteSpace(protectionCode),
                securityCodeChecker.IsEnableSecurityCode());
        }
        
        public async Task<ResultModel> Handle(UpdateProtectionCodeCommand message)
            => await protectionCodeService.UpdateAsync(new UpdateProtectionCodeParam(
                message.ProtectionCode,
                HashKey,
                message.CustId,
                message.SubAccId,
                message.LangKey,
                message.IP));

        private string HashKey
        {
            get
            {
                var hashKey = sessionManager.Get<string>(HashKeySessionKey);

                if (string.IsNullOrWhiteSpace(hashKey))
                {
                    hashKey = protectionCodeService.GenerateHashKey();
                    sessionManager.Add(HashKeySessionKey, hashKey);
                }

                return hashKey;
            }
        }
    }
}