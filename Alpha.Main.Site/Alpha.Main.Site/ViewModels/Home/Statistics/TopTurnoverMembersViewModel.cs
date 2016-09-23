namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using System.Collections.Generic;

    public class TopTurnoverMembersViewModel
    {
        IEnumerable<TopTurnoverMember> members;

        public TopTurnoverMembersViewModel(IEnumerable<TopTurnoverMember> members)
        {
            this.members = members;
        }

        public TopTurnoverMembersViewModel() : this(null)
        {
        }

        public IEnumerable<TopTurnoverMember> Members { get { return this.members; } }
    }
}