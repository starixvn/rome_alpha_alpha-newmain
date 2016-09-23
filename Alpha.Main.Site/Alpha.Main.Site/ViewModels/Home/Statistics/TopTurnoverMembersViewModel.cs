namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class TopTurnoverMembersViewModel
    {
        private IEnumerable<TopTurnoverMember> members;

        public TopTurnoverMembersViewModel(IEnumerable<TopTurnoverMember> members)
        {
            this.members = members;
        }

        public TopTurnoverMembersViewModel() : this(null)
        {
        }

        public IEnumerable<TopTurnoverMember> Members
        {
            get
            {
                return this.members;
            }
        }
    }
}