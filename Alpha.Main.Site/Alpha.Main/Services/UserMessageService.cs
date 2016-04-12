namespace Alpha.Main.Services
{
    using System.Collections.Generic;

    public interface UserMessageService
    {
        IEnumerable<string> GetHotMessages(string hotMessageFilePath, byte roleId, string language);
    }
}