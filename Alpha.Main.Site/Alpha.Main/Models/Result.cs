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

        public virtual bool IsFailed
        {
            get
            {
                return !IsSuccessful;
            }
        }

        public virtual bool IsOk
        {
            get
            {
                return Code == 0;
            }
        }

        public virtual bool IsNotOk
        {
            get
            {
                return !IsOk;
            }
        }

        public static Result CreateErrorResult(string message, int? code = null)
        {
            return new Result { Code = code ?? ErrorCode, Message = message };
        }

        public static Result CreateSuccessfulResult(string message = "")
        {
            return new Result { Code = 0, Message = message };
        }
    }
}