namespace Alpha.Main.Site.Models.Requests.UserMessages
{
    using System.Collections.Generic;
    using System.Web;
    using MediatR;

    public class HotMessagesQuery : IRequest<IEnumerable<string>>
    {
        private const string MessageSourcePath = "~/App_Data/UserMessage/ToolTipMessage.xml";
        private string hotMessageFilePath;

        public HotMessagesQuery() : this(null)
        {
        }

        internal HotMessagesQuery(string hotMessageFilePath)
        {
            this.hotMessageFilePath = hotMessageFilePath;
        }

        public string HotMessageFilePath => hotMessageFilePath ?? HttpContext.Current.Server.MapPath(MessageSourcePath);        

        public string Language { get; set; }

        public byte RoleId { get; set; }
    }
}