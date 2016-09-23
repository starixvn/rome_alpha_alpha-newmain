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

        public int CustId { get; set; }

        public string UserName { get; private set; }

        public decimal GivenCredit { get; private set; }

        public DateTime CreatedDate { get; private set; }
    }
}