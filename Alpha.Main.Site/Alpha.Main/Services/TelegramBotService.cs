namespace Alpha.Main.Services
{
    using System.Threading.Tasks;

    public interface TelegramBotService
    {
        Task NotifyChangePassword(int custId, int subAccId);

        Task NotifyChangePasswordAsync(int custId, int subAccId);

        Task NotifyChangeSecurityCode(int custId, int subAccId);

        Task NotifyChangeSecurityCodeAsync(int custId, int subAccId);
    }
}
