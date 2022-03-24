namespace Alpha.Main.Site.Requests.TelegramChatBot
{
    using Alpha.Main.Site.ViewModels.TelegramChatBot;
    using MediatR;

    public class GenerateQRCodeCommand : IAsyncRequest<VerifyAccountViewModel>
    {
        public GenerateQRCodeCommand(int customerLevel, int custId, int subAccId)
        {
            CustomerLevel = customerLevel;
            CustId = custId;
            SubAccId = subAccId;
        }

        public int CustomerLevel { get; }

        public int CustId { get; }

        public int SubAccId { get; }
    }
}