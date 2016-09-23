namespace Alpha.Main.Site.ViewModels.Home.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

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

        public IEnumerable<NewMember> Members
        {
            get
            {
                return this.members;
            }
        }
    }
}