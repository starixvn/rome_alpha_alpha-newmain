namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Services;
    using Alpha.Main.Site.Requests.UMMessage;
    using MediatR;

    public class UMMessageHandler : IAsyncRequestHandler<UMMessageQuery, IEnumerable<UMMessageItem>>
    {
        private readonly UMMessageService messageService;

        public UMMessageHandler(UMMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task<IEnumerable<UMMessageItem>> Handle(UMMessageQuery message)
            => await messageService.GetMessagesAsync(message.Language);
    }
}