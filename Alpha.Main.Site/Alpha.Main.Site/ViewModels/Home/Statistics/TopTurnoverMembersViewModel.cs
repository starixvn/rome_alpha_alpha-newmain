namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class TopTurnoverMembersViewModel
    {
        public TopTurnoverMembersViewModel(IEnumerable<TopTurnoverMember> members)
        {
            Members = members;
        }

        public TopTurnoverMembersViewModel() : this(null)
        {
        }

        public IEnumerable<TopTurnoverMember> Members { get; private set; }
    }
}