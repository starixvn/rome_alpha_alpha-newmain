namespace Alpha.Main.Models.Statistics
{
    public class Statistics
    {
        public Statistics(
            decimal outStanding,
            decimal totalOutStanding,
            decimal casinoTransfer,
            decimal casinoOutStanding,
            decimal bingoTransfer,
            decimal bingoOutstanding,
            decimal p2pTransfer,
            decimal p2pOutstanding,
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
            decimal casinoTransfer,
            decimal casinoOutstanding,
            decimal bingoTransfer,
            decimal bingoOutstanding,
            decimal p2pTransfer,
            decimal p2pOutstanding,
            int totalMemberActive,
            int totalMemberClosed,
            int totalMemberSuspended,
            int totalAgentActive,
            int totalAgentClosed,
            int totalAgentSuspended,
            int numberNewMembers)
        {
            Outstanding = outStanding;
            TotalOutstanding = totalOutStanding;
            TotalMemberActive = totalMemberActive;
            TotalMemberClosed = totalMemberClosed;
            TotalMemberSuspended = totalMemberSuspended;
            NumberNewMembers = numberNewMembers;
            TotalAgentActive = totalAgentActive;
            TotalAgentClosed = totalAgentClosed;
            TotalAgentSuspended = totalAgentSuspended;
        }

        public Statistics(
            decimal outStanding,
            decimal totalOutStanding,
            decimal casinoTransfer,
            decimal casinoOutstanding,
            decimal bingoTransfer,
            decimal bingoOutstanding,
            decimal p2pTransfer,
            decimal p2pOutstanding,
            int totalMemberActive,
            int totalMemberClosed,
            int totalMemberSuspended,
            int totalAgentActive,
            int totalAgentClosed,
            int totalAgentSuspended,
            int totalMasterActive,
            int totalMasterClosed,
            int totalMasterSuspended,
            int numberNewMembers)
        {
            Outstanding = outStanding;
            TotalOutstanding = totalOutStanding;
            TotalMemberActive = totalMemberActive;
            TotalMemberClosed = totalMemberClosed;
            TotalMemberSuspended = totalMemberSuspended;
            NumberNewMembers = numberNewMembers;
            TotalAgentActive = totalAgentActive;
            TotalAgentClosed = totalAgentClosed;
            TotalAgentSuspended = totalAgentSuspended;
            TotalMasterActive = totalMasterActive;
            TotalMasterClosed = totalMasterClosed;
            TotalMasterSuspended = totalMasterSuspended;
        }

        public decimal Outstanding { get; private set; }

        public decimal TotalOutstanding { get; private set; }

        public int TotalMemberActive { get; private set; }

        public int TotalMemberClosed { get; private set; }

        public int TotalMemberSuspended { get; private set; }

        public int TotalAgentActive { get; private set; }

        public int TotalAgentClosed { get; private set; }

        public int TotalAgentSuspended { get; private set; }

        public int TotalMasterActive { get; private set; }

        public int TotalMasterClosed { get; private set; }

        public int TotalMasterSuspended { get; private set; }

        public int NumberNewMembers { get; private set; }
    }
}