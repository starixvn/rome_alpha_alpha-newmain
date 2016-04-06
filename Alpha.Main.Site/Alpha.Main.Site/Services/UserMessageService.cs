namespace Alpha.Main.Site.Services
{
    using System.Collections.Generic;
    using Models.Requests.UserMessages;

    public interface UserMessageService
    {
        IEnumerable<string> GetHotMessages(HotMessagesQuery query);
    }
}