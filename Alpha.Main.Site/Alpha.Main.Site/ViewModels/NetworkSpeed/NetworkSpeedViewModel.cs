namespace Alpha.Main.Site.ViewModels.NetworkSpeed
{
    public class NetworkSpeedViewModel
    {
        public NetworkSpeedViewModel(bool requiredCalculation)
        {
            RequiredCalculation = requiredCalculation;
        }

        public bool RequiredCalculation { get; }
    }
}