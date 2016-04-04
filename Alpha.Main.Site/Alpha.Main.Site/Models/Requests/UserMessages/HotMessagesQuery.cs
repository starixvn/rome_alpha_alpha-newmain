namespace Alpha.Main.Site.Models.Requests.UserMessages
{
    using System.Collections.Generic;
    using MediatR;

    public class HotMessagesQuery : IRequest<IEnumerable<string>>
    {
        public string HotMessageFilePath { get; set; }

        public string Language { get; set; }

        public byte RoleId { get; set; }
    }
}