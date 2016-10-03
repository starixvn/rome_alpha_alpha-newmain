namespace Alpha.Main.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        public List<NoticeMessage> GetNoticeMessages(
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

            var message = dynamicRepository.Fetch<NoticeMessage>(criteria).ToList();

            return message;
        }

        public bool MarkMessageAsRead(int custId, string messageIds)
        {
            var criteria = new MarkMessageAsReadCriteria()
            {
                UserId = custId,
                MessageIds = messageIds
            };

            if (!criteria.IsValid())
            {
                return false;
            }

            dynamicRepository.Execute(criteria);

            return true;
        }
    }
}