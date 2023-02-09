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

        public async Task NotifyChangePassword(int custId, int subAccId)
            => await NotifyChangePasswordAsync(custId, subAccId);

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

        public async Task NotifyChangeSecurityCode(int custId, int subAccId)
            => await NotifyChangeSecurityCodeAsync(custId, subAccId);

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
