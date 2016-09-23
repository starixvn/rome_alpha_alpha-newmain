namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using Main.Models.Statistics;
    using System.Collections.Generic;

    public class NewMembersViewModel
    {
        private IEnumerable<NewMember> members;

        public NewMembersViewModel(IEnumerable<NewMember> members)
        {
            this.members = members;
        }

        public NewMembersViewModel() : this(null)
        {
        }

        public IEnumerable<NewMember> Members { get { return this.members; } }
    }
}