namespace Alpha.NewMain.Site.Models.Dtos.Statistics
{
    public class StatisticsModel
    {
        public decimal CasinoTransfer { get; set; }

        public decimal CasinoOutstanding { get; set; }

        public decimal BingoTransfer { get; set; }

        public decimal BingoOutstanding { get; set; }

        public decimal P2PTransfer { get; set; }

        public decimal P2POutstanding { get; set; }

        public decimal TotalMasterActive { get; set; }

        public int TotalMasterClosed { get; set; }

        public int TotalMasterSuspended { get; set; }

        public decimal TotalAgentActive { get; set; }

        public int TotalAgentClosed { get; set; }

        public int TotalAgentSuspended { get; set; }

        public decimal TotalMemberActive { get; set; }

        public int TotalMemberClosed { get; set; }

        public int TotalMemberSuspended { get; set; }

        public decimal Outstanding { get; set; }

        public decimal TotalOutstanding { get; set; }

        public int NumberNewMembers { get; set; }
    }
}