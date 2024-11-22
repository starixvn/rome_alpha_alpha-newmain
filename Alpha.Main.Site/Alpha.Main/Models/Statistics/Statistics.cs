namespace Alpha.Main.Models.Statistics
{
    public class Statistics
    {
        public decimal Outstanding { get; set; }

        public decimal TotalOutstanding { get; set; }

        public int TotalMemberActive { get; set; }

        public int TotalMemberClosed { get; set; }

        public int TotalMemberSuspended { get; set; }

        public int TotalAgentActive { get; set; }

        public int TotalAgentClosed { get; set; }

        public int TotalAgentSuspended { get; set; }

        public int TotalMasterActive { get; set; }

        public int TotalMasterClosed { get; set; }

        public int TotalMasterSuspended { get; set; }

        public int NumberNewMembers { get; set; }

        public int TotalDirectMemberActive { get; set; }

        public int TotalDirectMemberClosed { get; set; }

        public int TotalDirectMemberSuspended { get; set; }
    }
}