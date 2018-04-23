namespace Alpha.Main.Site.Requests.NetworkSpeed
{
    using MediatR;
    using ViewModels.NetworkSpeed;

    public class SaveNetworkSpeedCommand : IRequest<Unit>
    {
        public float SpeedKbps { get; set; }
    }
}