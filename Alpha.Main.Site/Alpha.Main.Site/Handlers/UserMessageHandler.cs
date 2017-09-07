namespace Alpha.Main.Site.Handlers
{
    using MediatR;
    using Models.Requests.UserMessages;
    using System.Collections.Generic;
    using System.Linq;

    public class UserMessageHandler :
        IRequestHandler<HotMessagesQuery, IEnumerable<string>>
    {
        public IEnumerable<string> Handle(HotMessagesQuery message)
        {
            return Enumerable.Empty<string>();
        }
    }
}