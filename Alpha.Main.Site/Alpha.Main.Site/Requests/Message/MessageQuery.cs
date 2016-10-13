namespace Alpha.Main.Site.Requests.Message
{
    using System;
    using System.Collections.Generic;
    using Main.Models.Enumerations;
    using MediatR;
    using Sunplus.Agent.Model.Entities;
    using ViewModels.Message;

    public class MessageQuery : IRequest<MessagesViewModel>
    {
        private const int DefaultTotalMessage = 50;

        private static readonly DateTime MinDateTime = new DateTime(1900, 1, 1);

        private static readonly Dictionary<byte, string> ParentSitesByLevel = new Dictionary<byte, string>
        {
            [UserLevel.Super.Value] = "3000,3100",
            [UserLevel.Master.Value] = "3000,3200",
            [UserLevel.Agent.Value] = "3000,3300"
        };

        private static readonly Dictionary<int, int> SpecialTypes = new Dictionary<int, int>
        {
            [Constants.MessageType.Public] = 0,
            [Constants.MessageType.Special] = 1,
            [Constants.MessageType.Private] = 2
        };

        public MessageQuery(
            IUserSession userSession,
            int messageType,
            DateTime startDate,
            DateTime endDate,
            bool isTest,
            int totalMessage = DefaultTotalMessage)
        {
            CustId = userSession.CustId;
            ParentSites = ParentSitesByLevel[userSession.RoleId];
            MessageType = messageType;
            SpecialType = SpecialTypes[messageType];
            StartDate = MessageType == Constants.MessageType.Special ? MinDateTime : startDate;
            EndDate = endDate;
            IsTest = isTest;
            TotalMessage = totalMessage;
            CurrentLang = userSession.Language;
            RoleId = userSession.RoleId;
        }

        public int CustId { get; }

        public string ParentSites { get; }

        public int MessageType { get; }

        public int SpecialType { get; }

        public int TotalMessage { get; } = DefaultTotalMessage;

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }

        public bool IsTest { get; }

        public string CurrentLang { get; }

        public byte RoleId { get; }
    }
}