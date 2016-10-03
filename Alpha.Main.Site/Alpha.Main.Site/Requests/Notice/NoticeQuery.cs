#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Notice
{
    using System;
    using System.Collections.Generic;
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
            this.LastPostTime = lastPostTime;
            this.TotalMessage = totalMessage;
            this.IsTest = isTest;
        }

        public UserLevel UserLevel
        {
            get
            {
                return Enumeration.FromValue<UserLevel>(this.userSession.RoleId);
            }
        }

        public int CustId
        {
            get
            {
                return this.userSession.CustId;
            }
        }

        public string ParentSiteId
        {
            get
            {
                var siteId = GetSiteIdByUserLevel().ToString();

                return GetParentSitesId(siteId);
            }
        }

        public DateTime LastPostTime { get; private set; } = DateTime.Today;

        public int TotalMessage { get; private set; } = 10;

        public bool IsTest { get; private set; } = false;

        public string CurrentLang
        {
            get
            {
                return this.userSession.Language;
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

        private string GetParentSitesId(string siteId)
        {
            var parentSiteIds = new List<string>();
            int num = 0;

            for (int i = siteId.Length - 1; i >= 0; i--)
            {
                if (siteId[i] != '0')
                {
                    num = i;
                    break;
                }
            }

            for (int j = 1; j <= num; j++)
            {
                string str = string.Empty;

                for (int m = j; m < siteId.Length; m++)
                {
                    str += "0";
                }

                parentSiteIds.Add(siteId.Remove(j) + str);
            }

            string parentSiteIdsStr = string.Empty;

            foreach (string parentSiteId in parentSiteIds)
            {
                parentSiteIdsStr += parentSiteId + ",";
            }

            return parentSiteIdsStr + siteId;
        }
    }
}