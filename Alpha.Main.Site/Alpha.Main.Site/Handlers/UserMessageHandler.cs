namespace Alpha.Main.Site.Handlers
{
    using System.Collections.Generic;
    using System.Linq;
    using MediatR;
    using Models.Requests.UserMessages;

    public class UserMessageHandler :
        IRequestHandler<HotMessagesQuery, IEnumerable<string>>
    {
        public IEnumerable<string> Handle(HotMessagesQuery message)
        {
            return Enumerable.Empty<string>();
        }
    }
}