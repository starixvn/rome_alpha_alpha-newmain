namespace Alpha.Main.Models
{
    public class Result
    {
        public const int ErrorCode = 1;

        public int Code { get; set; }

        public string Message { get; set; }

        public virtual bool IsSuccessful
        {
            get
            {
                return Code == 0;
            }
        }

        public virtual bool IsOK
        {
            get
            {
                return Code == 0;
            }
        }
    }
}