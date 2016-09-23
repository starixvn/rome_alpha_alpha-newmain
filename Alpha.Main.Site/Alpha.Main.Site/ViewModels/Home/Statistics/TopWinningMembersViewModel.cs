namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using Main.Models.Enumerations;
    using Main.Models.Statistics;
    using System.Collections.Generic;

    public class TopWinningMembersViewModel
    {
        private IEnumerable<TopWinningMember> members;

        public TopWinningMembersViewModel(IEnumerable<TopWinningMember> members)
        {
            this.members = members;
        }

        public TopWinningMembersViewModel() : this(null)
        {
        }

        public IEnumerable<TopWinningMember> Members { get { return this.members; } }
    }
}