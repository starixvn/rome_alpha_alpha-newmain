namespace Alpha.Main.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Main.Models.UMMessage;

    public interface UMMessageService
    {
        Task<IEnumerable<UMMessageItem>> GetMessagesAsync(string language);
    }
}
