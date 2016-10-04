namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using Main.Models.Notice;
    using MediatR;
    using Requests.Notice;
    using Services;
    using ViewModels.Dashboard.Notice;

    public class NoticeHandler :
        IRequestHandler<NoticeQuery, NoticeViewModel>,
        IRequestHandler<MarkMessageAsReadQuery, bool>
    {
        private NoticeService noticeService;

        public NoticeHandler(NoticeService noticeService)
        {
            this.noticeService = noticeService;
        }

        public NoticeViewModel Handle(NoticeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var noticeMessages = noticeService.GetNoticeMessages(
                                    message.CustId,
                                    message.ParentSiteId,
                                    message.LastPostTime,
                                    message.TotalMessage,
                                    message.IsTest);

            var noticeMessageViewModels = CreateNoticeMessageViewModels(noticeMessages, message.CurrentLang);

            var viewModel = new NoticeViewModel(noticeMessageViewModels);

            return viewModel;
        }

        public bool Handle(MarkMessageAsReadQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), "Query cannot be null");
            }

            var isSuccess = noticeService.MarkMessageAsRead(message.CustId, message.MessageId);

            return isSuccess;
        }

        private List<NoticeMessageWrapper> CreateNoticeMessageViewModels(List<NoticeMessage> noticeMessages, string currentLang)
        {
            var noticeMessageViewWrappers = new List<NoticeMessageWrapper>();

            foreach (var noticeMessage in noticeMessages)
            {
                var noticeMessageWrapper = new NoticeMessageWrapper(noticeMessage, currentLang);
                noticeMessageViewWrappers.Add(noticeMessageWrapper);
            }

            return noticeMessageViewWrappers;
        }
    }
}