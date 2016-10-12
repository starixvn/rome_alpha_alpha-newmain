namespace Alpha.Main.Models.Statistics
{
    public class Statistics
    {
        public Statistics(
            decimal outStanding,
            decimal totalOutStanding,
            int totalMemberActive,
            int totalMemberClosed,
            int totalMemberSuspended,
            int numberNewMembers)
        {
            Outstanding = outStanding;
            TotalOutstanding = totalOutStanding;
            TotalMemberActive = totalMemberActive;
            TotalMemberClosed = totalMemberClosed;
            TotalMemberSuspended = totalMemberSuspended;
            NumberNewMembers = numberNewMembers;
        }

        public Statistics(
            decimal outStanding,
            decimal totalOutStanding,
            int totalMemberActive,
            int totalMemberClosed,
            int totalMemberSuspended,
            int totalAgentActive,
            int totalAgentClosed,
            int totalAgentSuspended,
            int numberNewMembers) : this(
                    outStanding,
                    totalOutStanding,
                    totalMemberActive,
                    totalMemberClosed,
                    totalMemberSuspended,
                    numberNewMembers)
        {
            TotalAgentActive = totalAgentActive;
            TotalAgentClosed = totalAgentClosed;
            TotalAgentSuspended = totalAgentSuspended;
        }

        public Statistics(
            decimal outStanding,
            decimal totalOutStanding,
            int totalMemberActive,
            int totalMemberClosed,
            int totalMemberSuspended,
            int totalAgentActive,
            int totalAgentClosed,
            int totalAgentSuspended,
            int totalMasterActive,
            int totalMasterClosed,
            int totalMasterSuspended,
            int numberNewMembers) : this(
                    outStanding,
                    totalOutStanding,
                    totalMemberActive,
                    totalMemberClosed,
                    totalMemberSuspended,
                    totalAgentActive,
                    totalAgentClosed,
                    totalAgentSuspended,
                    numberNewMembers)
        {
            TotalMasterActive = totalMasterActive;
            TotalMasterClosed = totalMasterClosed;
            TotalMasterSuspended = totalMasterSuspended;
        }

        public decimal Outstanding { get; }

        public decimal TotalOutstanding { get; }

        public int TotalMemberActive { get; }

        public int TotalMemberClosed { get; }

        public int TotalMemberSuspended { get; }

        public int TotalAgentActive { get; }

        public int TotalAgentClosed { get; }

        public int TotalAgentSuspended { get; }

        public int TotalMasterActive { get; }

        public int TotalMasterClosed { get; }

        public int TotalMasterSuspended { get; }

        public int NumberNewMembers { get; }
    }
}