namespace Alpha.Main.Services.Impl
{
    using Fanex.Customer.Client.Params;
    using Fanex.Customer.Client.Services;
    using Models;
    using Params.Nickname;
    using System.Net;
    using System.Threading.Tasks;

    public class NicknameServiceImpl : NicknameService
    {   
        private readonly ICustomerService customerService;

        public NicknameServiceImpl(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        public async Task<Result> CreateNickname(CreateNicknameParam param, bool nickNameLoginOnly)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            if (param.IsEnableUpdateNickname)
            {
                var nickNameRequest = new CreateNickNameParam(param.CustId, param.SubAccountId, param.Nickname, param.IP, param.Site, param.LanguageKey);
                var result = param.IsSubAccount ?
                    await customerService.CreateNickNameOfSubAcc(nickNameRequest)
                    : await customerService.CreateNickName(nickNameRequest);
                return new Result { Code = result.Code, Message = result.Message };
            }

            return new Result();
        }        
    }
}
