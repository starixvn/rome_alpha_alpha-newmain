namespace Alpha.NewMain.Site.Models.Dtos
{
    using System;

    public class CustomerInfoModel
    {
        public string Username { get; set; }

        public DateTime CreatedDate { get; set; }

        public decimal WinLoss { get; set; }

        public decimal Turnover { get; set; }

        public decimal GivenCredit { get; set; }
    }
}