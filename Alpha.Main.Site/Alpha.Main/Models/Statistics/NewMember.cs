namespace Alpha.Main.Models.Statistics
{
    using System;

    public class NewMember
    {
        public NewMember(int custId, string userName, DateTime createdDate, decimal givenCredit)
        {
            CustId = custId;
            UserName = userName;
            GivenCredit = givenCredit;
            CreatedDate = createdDate;
        }

        public int CustId { get; }

        public string UserName { get; }

        public decimal GivenCredit { get; }

        public DateTime CreatedDate { get; }
    }
}