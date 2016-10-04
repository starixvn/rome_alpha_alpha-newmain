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

    public class NoticeQuery : IRequest<NoticeViewModel>
    {
        private IUserSession userSession;

        public NoticeQuery(
            IUserSession userSession,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            this.userSession = userSession;
            LastPostTime = lastPostTime;
            TotalMessage = totalMessage;
            IsTest = isTest;
        }

        public UserLevel UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(userSession.RoleId);
            }
        }

        public int CustId
        {
            get
            {
                return userSession.CustId;
            }
        }

        public string ParentSiteId
        {
            get
            {
                var siteId = GetSiteIdByUserLevel().ToString();

                return GetParentSiteIds(siteId);
            }
        }

        public DateTime LastPostTime { get; private set; } = DateTime.Today;

        public int TotalMessage { get; private set; } = 10;

        public bool IsTest { get; private set; } = false;

        public string CurrentLang
        {
            get
            {
                return userSession.Language;
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

        private string GetParentSiteIds(string siteId)
        {
            var parentSiteIds = new List<string>();

            int lastIndexOfNotZeroChar = siteId.ToList().FindLastIndex(c => c != '0');

            for (int i = 1; i <= lastIndexOfNotZeroChar; i++)
            {
                var parentSiteId = siteId.Remove(i) + new string('0', siteId.Length - i);

                parentSiteIds.Add(parentSiteId);
            }

            parentSiteIds.Add(siteId);

            return string.Join(",", parentSiteIds);
        }
    }
}