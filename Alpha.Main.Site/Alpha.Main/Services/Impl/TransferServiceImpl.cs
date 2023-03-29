namespace Alpha.Main.Services.Impl
{
    using Alpha.Main.Criteria.Transfer;
    using Alpha.Main.DataAccesses;
    using Alpha.Main.DataAccesses.Impl;

    public class TransferServiceImpl : TransferService
    {
        private readonly TransferDataAccess transferDataAccess;

        public TransferServiceImpl(TransferDataAccess transferDataAccess = null)
            => this.transferDataAccess = transferDataAccess ?? new TransferDataAccessImpl();

        public bool IsAllDayBalanceTransfer(int custId)
            => transferDataAccess.IsAllDayBalanceTransfer(new GetIsAllDayBalTransferCriteria(custId));
    }
}
