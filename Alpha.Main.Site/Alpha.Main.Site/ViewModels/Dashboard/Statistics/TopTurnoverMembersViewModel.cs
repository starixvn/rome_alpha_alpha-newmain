namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class TopTurnoverMembersViewModel
    {
        public TopTurnoverMembersViewModel(List<TopTurnoverMember> members)
        {
            Members = members;
        }

        public List<TopTurnoverMember> Members { get; private set; }
    }
}