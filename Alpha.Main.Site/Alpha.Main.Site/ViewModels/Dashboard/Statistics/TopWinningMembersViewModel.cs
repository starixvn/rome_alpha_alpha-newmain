namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class TopWinningMembersViewModel
    {
        public TopWinningMembersViewModel(List<TopWinningMember> members)
        {
            Members = members;
        }

        public List<TopWinningMember> Members { get; private set; }
    }
}