namespace Alpha.Main.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alpha.Main.Models.UMMessage;
    using Alpha.Main.Params.UMMessage;

    public interface UMMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync(UMMessageParam param);
    }
}
