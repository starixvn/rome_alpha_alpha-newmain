namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class TopWinningMembersViewModel
    {
        public TopWinningMembersViewModel(IEnumerable<TopWinningMember> members)
        {
            Members = members;
        }

        public IEnumerable<TopWinningMember> Members { get; }
    }
}