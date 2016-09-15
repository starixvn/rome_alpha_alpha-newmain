namespace Alpha.Main.Models.SecurityCode
{
    using System;

    /// <summary>
    /// Result Entity.
    /// </summary>
    [Serializable]
    public class Result
    {
        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

    }
}