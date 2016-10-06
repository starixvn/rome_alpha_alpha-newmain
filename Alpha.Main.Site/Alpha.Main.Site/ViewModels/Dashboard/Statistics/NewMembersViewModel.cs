namespace Alpha.Main.Site.ViewModels.Dashboard.Statistics
{
    using System.Collections.Generic;
    using Main.Models.Statistics;

    public class NewMembersViewModel
    {
        public NewMembersViewModel(List<NewMember> members)
        {
            Members = members;
        }

        public List<NewMember> Members { get; }
    }
}