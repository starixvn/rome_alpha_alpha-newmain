namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Params.UMMessage;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.UMMessage;
    using MediatR;

    public class UMMessageHandler : IAsyncRequestHandler<UMMessageQuery, IEnumerable<UMMessageDTO>>
    {
        private readonly UMMessageService messageService;

        public UMMessageHandler(UMMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task<IEnumerable<UMMessageDTO>> Handle(UMMessageQuery message)
            => await messageService.GetMessagesAsync(ToUMMessageParam(message));

        private static UMMessageParam ToUMMessageParam(UMMessageQuery message)
            => new UMMessageParam(message.CustId, message.Site, message.CurrencyId, message.SuperId);
    }
}