namespace Alpha.Main.Site.Models.Requests.UserMessages
{
    using MediatR;
    using System.Collections.Generic;

    public class HotMessagesQuery : IRequest<IEnumerable<string>>
    {       
        public string Language { get; set; }

        public byte RoleId { get; set; }
    }
}