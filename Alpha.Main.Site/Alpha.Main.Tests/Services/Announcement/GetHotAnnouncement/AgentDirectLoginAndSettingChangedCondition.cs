namespace Alpha.Main.Tests.Services.Announcement.GetHotAnnouncement
{
    using Alpha.Main.Models.Enumerations;
    using Criteria.Announcement;
    using Entities.Announcements;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Params.Announcements;
    using System.Collections.Generic;

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
            DynamicRepository.Fetch<string>(Arg.Any<GetMessageTypeListCriteria>()).Returns(settingChanges);

            IEnumerable<PersonalMessage> insertPersonalMessageResult = IsInsertPersonalMessage ?
                   new List<PersonalMessage> { new PersonalMessage { Id = 0 } } :
                   null;
            DynamicRepository.Fetch<PersonalMessage>(Arg.Any<InsertPersonalMessageCriteria>()).Returns(insertPersonalMessageResult);
        }

        private List<string> GetSettingChanges(bool isPositionTakingChanged, bool isBettingLimitChanged)
        {
            var settingChanges = new List<string>();
            if (isPositionTakingChanged)
            {
                settingChanges.Add(PositionTakingChanged.ToString());
            }

            if (isBettingLimitChanged)
            {
                settingChanges.Add(BettingLimitChanged.ToString());
            }

            return settingChanges;
        }
    }
}
