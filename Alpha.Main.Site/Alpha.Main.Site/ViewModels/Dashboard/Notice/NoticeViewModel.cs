namespace Alpha.Main.Site.ViewModels.Dashboard.Notice
{
    public class NoticeViewModel
    {
        public NoticeViewModel(int totalMessage = 0)
        {
            TotalMessage = totalMessage;
        }

        public int TotalMessage { get; }
    }
}