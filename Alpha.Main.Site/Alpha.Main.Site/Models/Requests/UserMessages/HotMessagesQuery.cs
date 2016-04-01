namespace Alpha.Main.Site.Models.Requests.UserMessages
{
    using System.Collections.Generic;
    using MediatR;

    public class HotMessagesQuery : IRequest<IEnumerable<string>>
    {
    }
}