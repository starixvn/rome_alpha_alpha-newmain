namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class TopTurnoverMembersViewModel
    {
        public TopTurnoverMembersViewModel(IEnumerable<TopTurnoverMember> members)
        {
            Members = members;
        }

        public IEnumerable<TopTurnoverMember> Members { get; }
    }
}