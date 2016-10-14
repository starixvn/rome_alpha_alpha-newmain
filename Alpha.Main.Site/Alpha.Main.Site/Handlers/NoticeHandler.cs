namespace Alpha.Main.Site.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Notice;
    using MediatR;
    using Requests.Dashboard.Notice;
    using Services;
    using ViewModels.Dashboard.Notice;

    public class NoticeHandler :
        IRequestHandler<NoticeQuery, NoticeMessagesViewModel>,
        IRequestHandler<MarkMessageAsReadQuery, bool>
    {
        private const string QueryNullExceptionMessage = "Query cannot be null";
        private readonly NoticeService noticeService;

        public NoticeHandler(NoticeService noticeService)
        {
            this.noticeService = noticeService;
        }

        public NoticeMessagesViewModel Handle(NoticeQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), QueryNullExceptionMessage);
            }

            var noticeMessages = noticeService.GetNoticeMessages(
                                    message.CustId,
                                    message.ParentSites,
                                    message.LastPostTime,
                                    message.TotalMessage,
                                    message.IsTest);

            var noticeMessageWrappers = new List<NoticeMessageWrapper>(
                noticeMessages.Select(m => new NoticeMessageWrapper(m, message.CurrentLang)));

            var viewModel = new NoticeMessagesViewModel(noticeMessageWrappers);

            return viewModel;
        }

        public bool Handle(MarkMessageAsReadQuery message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message), QueryNullExceptionMessage);
            }

            var isSuccess = noticeService.MarkMessageAsRead(message.CustId, message.MessageId);

            return isSuccess;
        }
    }
}