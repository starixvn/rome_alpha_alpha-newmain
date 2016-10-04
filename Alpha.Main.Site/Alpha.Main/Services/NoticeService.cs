namespace Alpha.Main.Services
{
    using System;
    using System.Collections.Generic;
    using Models.Notice;

    public interface NoticeService
    {
        List<NoticeMessage> GetNoticeMessages(
            int targetId,
            string targetId2,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest);

        bool MarkMessageAsRead(int custId, int messageId);
    }
}