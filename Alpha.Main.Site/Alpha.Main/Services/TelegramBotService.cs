namespace Alpha.Main.Services
{
    using System.Threading.Tasks;

    public interface TelegramBotService
    {
        void NotifyChangePassword(int custId, int subAccId, string ip);

        Task NotifyChangePasswordAsync(int custId, int subAccId, string ip);

        void NotifyChangeSecurityCode(int custId, int subAccId, string ip);

        Task NotifyChangeSecurityCodeAsync(int custId, int subAccId, string ip);
    }
}
