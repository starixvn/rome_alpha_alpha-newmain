namespace Alpha.Main.Site.Requests.NetworkSpeed
{
    using MediatR;

    public class SaveNetworkSpeedCommand : IRequest<Unit>
    {
        public float SpeedKbps { get; set; }
    }
}