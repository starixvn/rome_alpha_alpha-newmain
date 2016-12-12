#pragma warning disable 618, 612

namespace Alpha.Main.Site.Requests.Dashboard.Notice
{
    using System;
    using System.Collections.Generic;
    using Configurations;
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Dashboard.Notice;

    public class NoticeQuery : IRequest<NoticeMessagesViewModel>
    {
        private readonly Dictionary<byte, string> parentSitesByLevel;

        private readonly byte roleId;

        public NoticeQuery(
            IUserSession userSession,
            DateTime lastPostTime,
            int totalMessage,
            bool isTest)
        {
            parentSitesByLevel = InitiateParentSitesByLevel();

            roleId = userSession.RoleId;
            CustId = userSession.CustId;
            CurrentLang = userSession.Language;
            LastPostTime = lastPostTime;
            TotalMessage = totalMessage;
            IsTest = isTest;
        }

        public UserLevel UserLevel => Enumeration.FromValue<UserLevel>(roleId);

        public int CustId { get; }

        public DateTime LastPostTime { get; } = DateTime.Today;

        public int TotalMessage { get; } = 10;

        public bool IsTest { get; }

        public string CurrentLang { get; }

        public string ParentSites => parentSitesByLevel[UserLevel.Value];

        private Dictionary<byte, string> InitiateParentSitesByLevel()
        {
            return new Dictionary<byte, string>
            {
                [UserLevel.Super.Value] = string.IsNullOrWhiteSpace(AppSettings.MessageSuper) ? "3000,3100" : AppSettings.MessageSuper,
                [UserLevel.Master.Value] = string.IsNullOrWhiteSpace(AppSettings.MessageMaster) ? "3000,3200" : AppSettings.MessageMaster,
                [UserLevel.Agent.Value] = string.IsNullOrWhiteSpace(AppSettings.MessageAgent) ? "3000,3300" : AppSettings.MessageAgent
            };
        }
    }
}