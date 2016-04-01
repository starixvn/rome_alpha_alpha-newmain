namespace Alpha.Main.Site.Services
{
    using System.Collections.Generic;

    public interface UserMessageService
    {
        IEnumerable<string> GetHotMessages(string hotMessageFilePath);
    }
}