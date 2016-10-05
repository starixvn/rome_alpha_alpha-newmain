#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Notice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.Notice;

    public class NoticeQuery : IRequest<NoticeMessagesViewModel>
    {
        public NoticeQuery(
            IUserSession userSession,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            UserLevel = Enumeration.FromValue<UserLevel>(userSession.RoleId);
            CustId = userSession.CustId;
            CurrentLang = userSession.Language;
            LastPostTime = lastPostTime;
            TotalMessage = totalMessage;
            IsTest = isTest;
        }

        public UserLevel UserLevel { get; private set; }

        public int CustId { get; private set; }

        public DateTime LastPostTime { get; private set; } = DateTime.Today;

        public int TotalMessage { get; private set; } = 10;

        public bool IsTest { get; private set; } = false;

        public string CurrentLang { get; private set; }

        public string ParentSiteId
        {
            get
            {
                var siteId = GetSiteIdByUserLevel();

                return GetParentSiteIds(siteId);
            }
        }

        private int GetSiteIdByUserLevel()
        {
            int siteId = 3000;

            if (UserLevel.IsSuperLevel())
            {
                siteId = 3100;
            }
            else if (UserLevel.IsMasterLevel())
            {
                siteId = 3200;
            }
            else if (UserLevel.IsAgentLevel())
            {
                siteId = 3300;
            }

            return siteId;
        }

        private string GetParentSiteIds(int siteId)
        {
            var parentSiteIds = (siteId == 3000) ? siteId.ToString() : $"3000,{siteId}";

            return parentSiteIds;
        }
    }
}