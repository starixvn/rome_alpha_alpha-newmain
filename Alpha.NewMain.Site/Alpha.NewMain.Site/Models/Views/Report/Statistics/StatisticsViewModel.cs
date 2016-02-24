namespace Alpha.NewMain.Site.Models.Views.Report.Statistics
{
    using System.Collections.Generic;

    public class StatisticsViewModel
    {
        public string CasinoTransfer { get; set; }

        public string CasinoOutstanding { get; set; }

        public string BingoTransfer { get; set; }

        public string BingoOutstanding { get; set; }

        public string P2PTransfer { get; set; }

        public string P2POutstanding { get; set; }

        public List<MemberStatusViewModel> MemberStatus { get; set; }

        public string Outstanding { get; set; }

        public string TotalOutstanding { get; set; }

        public string NumberNewMembers { get; set; }

        public string Label_Outstanding { get; set; }
    }
}