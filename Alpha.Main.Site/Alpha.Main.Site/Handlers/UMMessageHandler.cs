namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Params.UMMessage;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.UMMessage;
    using MediatR;

    public class UMMessageHandler : IAsyncRequestHandler<UMMessageQuery, IEnumerable<MessageDTO>>
    {
        private const int LockEditFunctionType = 1;

        private readonly UMMessageService messageService;

        public UMMessageHandler(UMMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task<IEnumerable<MessageDTO>> Handle(UMMessageQuery message)
            => await messageService.GetMessagesAsync(
                new UMMessageParam(message.CustId, LockEditFunctionType, message.Language));
    }
}