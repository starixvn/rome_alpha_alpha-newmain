namespace Alpha.Main.Site.Models.Requests.UserMessages
{
    using System.Collections.Generic;
    using System.Web;
    using MediatR;

    public class HotMessagesQuery : IRequest<IEnumerable<string>>
    {
        private string hotMessageFilePath;

        public HotMessagesQuery() : this(null)
        {
        }

        internal HotMessagesQuery(string hotMessageFilePath)
        {
            this.hotMessageFilePath = hotMessageFilePath;
        }

        public string HotMessageFilePath
        {
            get
            {
                return hotMessageFilePath ?? HttpContext.Current.Server.MapPath("~/App_Data/UserMessage/ToolTipMessage.xml");
            }
        }

        public string Language { get; set; }

        public byte RoleId { get; set; }
    }
}