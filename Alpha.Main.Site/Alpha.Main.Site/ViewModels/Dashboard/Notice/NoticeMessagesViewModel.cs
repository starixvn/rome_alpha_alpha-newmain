namespace Alpha.Main.Site.ViewModels.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Message;

    public class NoticeMessagesViewModel
    {
        public NoticeMessagesViewModel(List<NoticeMessageWrapper> noticeMessages)
        {
            NoticeMessages = noticeMessages
                .Where(m => m.CurrentMessage.TopLevel >= (byte)MessageLevel.Range.Normal)
                .OrderByDescending(m => m.CurrentMessage.PostTime)
                .ToList();

            HighlightMessages = noticeMessages
                .Where(m => m.CurrentMessage.TopLevel <= (byte)MessageLevel.Range.Highlight)
                .OrderBy(m => m.CurrentMessage.TopLevel)
                .ThenByDescending(m => m.PostTime)
                .ToList();

            LastMessagePostTime = noticeMessages.Any() ? noticeMessages.Last().PostTime : DateTime.MinValue;
        }

        public List<NoticeMessageWrapper> NoticeMessages { get; }

        public List<NoticeMessageWrapper> HighlightMessages { get; }

        public DateTime LastMessagePostTime { get; }
    }
}