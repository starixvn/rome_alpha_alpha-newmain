namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using Criteria.Notice;
    using Fanex.Data.Repository;
    using Models.Notice;

    public class NoticeServiceImpl : NoticeService
    {
        private IDynamicRepository dynamicRepository;

        public NoticeServiceImpl(
            IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<NoticeMessage> GetNoticeMessages(
            int targetId,
            string targetId2,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            var criteria = new GetNoticeMessagesCriteria
            {
                TargetId = targetId,
                TargetId2 = targetId2,
                LastPostTime = lastPostTime,
                TotalMessage = totalMessage,
                IsTest = isTest
            };

            var message = dynamicRepository.Fetch<NoticeMessage>(criteria);

            return message;
        }

        public bool MarkMessageAsRead(int custId, string messageIds)
        {
            var criteria = new MarkMessageAsReadCriteria()
            {
                UserId = custId,
                MessageIds = messageIds
            };

            try
            {
                dynamicRepository.Execute(criteria);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}