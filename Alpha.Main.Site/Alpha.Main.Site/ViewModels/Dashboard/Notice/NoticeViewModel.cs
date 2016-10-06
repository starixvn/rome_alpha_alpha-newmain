namespace Alpha.Main.Site.ViewModels.Dashboard.Notice
{
    public class NoticeViewModel
    {
        public NoticeViewModel(int totalMessage)
        {
            TotalMessage = totalMessage;
        }

        public int TotalMessage { get; }
    }
}