namespace Alpha.Main.Models.ApiKey
{
    public class Result
    {
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsFailed => Code != 0;
    }
}
