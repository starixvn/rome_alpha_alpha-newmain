namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using System.Collections.Generic;
    using Alpha.Main.Models.Enumerations;
    using Criteria.Announcement;
    using Fanex.Data.Repository;
    using Main.Models.Announcements;
    using NSubstitute;
    using Params.Announcements;

    public class AgentDirectLoginAndSettingChangedCondition : LogInAndSettingChangedCondition
    {
        private const int PositionTakingChanged = 1;
        private const int BettingLimitChanged = 2;

        public AgentDirectLoginAndSettingChangedCondition(
            bool isPositionTakingChanged = false,
            bool isBettingLimitChanged = false,
            bool isInsertPersonalMessage = false)
        {
            IsPositionTakingChanged = isPositionTakingChanged;
            IsBettingLimitChanged = isBettingLimitChanged;
            IsInsertPersonalMessage = isInsertPersonalMessage;

            RoleId = UserLevel.Agent.Value;
            AdminId = 0;
        }

        public bool IsPositionTakingChanged { get; set; }

        public bool IsBettingLimitChanged { get; set; }

        public bool IsInsertPersonalMessage { get; set; }

        public IDynamicRepository DynamicRepository { get; set; }

        public override void SetUpCondition(GetHotAnnouncementsParams query)
        {
            base.SetUpCondition(query);

            DynamicRepository = Substitute.For<IDynamicRepository>();
            var settingChanges = GetSettingChanges(IsPositionTakingChanged, IsBettingLimitChanged);
            DynamicRepository.Fetch<byte>(Arg.Any<GetMessageTypeListCriteria>()).Returns(settingChanges);

            IEnumerable<PersonalMessage> insertPersonalMessageResult = IsInsertPersonalMessage ?
                   new List<PersonalMessage> { new PersonalMessage { Id = 0 } } :
                   null;
            DynamicRepository.Fetch<PersonalMessage>(Arg.Any<InsertPersonalMessageCriteria>()).Returns(insertPersonalMessageResult);
        }

        private List<byte> GetSettingChanges(bool isPositionTakingChanged, bool isBettingLimitChanged)
        {
            var settingChanges = new List<byte>();
            if (isPositionTakingChanged)
            {
                settingChanges.Add(PositionTakingChanged);
            }

            if (isBettingLimitChanged)
            {
                settingChanges.Add(BettingLimitChanged);
            }

            return settingChanges;
        }
    }
}