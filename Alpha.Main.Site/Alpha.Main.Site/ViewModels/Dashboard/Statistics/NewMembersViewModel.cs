namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class NewMembersViewModel
    {
        public NewMembersViewModel(IEnumerable<NewMember> members)
        {
            this.Members = members;
        }

        public IEnumerable<NewMember> Members { get; private set; }
    }
}