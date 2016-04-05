namespace Alpha.Main.Site.Models.Requests.UserMessages
{
    using System.Collections.Generic;
    using System.Web;
    using MediatR;

    public class HotMessagesQuery : IRequest<IEnumerable<string>>
    {
        public HotMessagesQuery() : this(null)
        {
        }

        internal HotMessagesQuery(string hotMessageFilePath)
        {
            HotMessageFilePath = hotMessageFilePath ?? HttpContext.Current.Server.MapPath("~/App_Data/UserMessage/ToolTipMessage.xml");
        }

        public string HotMessageFilePath { get; set; }

        public string Language { get; set; }

        public byte RoleId { get; set; }
    }
}