namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging.Extensions.Web;
    using TelegramBot.Client.Services;

    public class TelegramBotServiceImpl : TelegramBotService
    {
        private readonly INotificationService notificationService;

        public TelegramBotServiceImpl(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public void NotifyChangePassword(int custId, int subAccId)
            => NotifyChangePasswordAsync(custId, subAccId).GetAwaiter().GetResult();

        public async Task NotifyChangePasswordAsync(int custId, int subAccId)
        {
            try
            {
                await notificationService.NotifyChangePassword(custId, subAccId);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }

        public void NotifyChangeSecurityCode(int custId, int subAccId)
            => NotifyChangeSecurityCodeAsync(custId, subAccId).GetAwaiter().GetResult();

        public async Task NotifyChangeSecurityCodeAsync(int custId, int subAccId)
        {
            try
            {
                await notificationService.NotifyChangeSecurityCode(custId, subAccId);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
