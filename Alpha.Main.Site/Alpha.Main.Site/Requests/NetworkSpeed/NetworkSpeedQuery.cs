namespace Alpha.Main.Site.Requests.NetworkSpeed
{
    using MediatR;
    using ViewModels.NetworkSpeed;

    public class NetworkSpeedQuery : IRequest<NetworkSpeedViewModel>
    {
        public NetworkSpeedQuery(int custId)
        {
            CustId = custId;
        }

        public int CustId { get; }
    }
}