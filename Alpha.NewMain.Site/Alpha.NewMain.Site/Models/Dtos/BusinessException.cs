namespace Alpha.NewMain.Site.Models.Dtos
{
    using System;

    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base("Business exception - " + message)
        {
        }
    }
}