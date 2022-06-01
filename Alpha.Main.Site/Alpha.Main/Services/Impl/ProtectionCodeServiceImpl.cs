namespace Alpha.Main.Services.Impl
{
    using System.Linq;
    using System.Threading.Tasks;
    using Alpha.Main.AppResources;
    using Alpha.Main.Criteria.ProtectionCode;
    using Alpha.Main.Models;
    using Alpha.Main.Params.ProtectionCode;
    using Fanex.Cryptography.Aes;
    using Fanex.Data.Repository;
    using Sunplus.Agent.Utilities;

    public class ProtectionCodeServiceImpl : ProtectionCodeService
    {
        private const int Length = 4;
        private readonly IAesService aes;
        private readonly Encryptor encryptor;
        private readonly IDynamicRepository repository;

        public ProtectionCodeServiceImpl(
            IAesService aes,
            Encryptor encryptor,
            IDynamicRepository repository)
        {
            this.aes = aes;
            this.encryptor = encryptor;
            this.repository = repository;
        }

        public string GenerateHashKey()
            => encryptor.RandomString(Length);

        public Task<string> GetAsync(int custId, int subAccId)
            => repository.GetAsync<string>(new GetProtectionCodeCriteria(custId, subAccId));

        public async Task<Result> UpdateAsync(UpdateProtectionCodeParam param)
        {
            if (string.IsNullOrWhiteSpace(param.ProtectionCode))
            {
                return Result.CreateErrorResult(Resources.RequiredNewProtectionCode);
            }

            var rawProtectionCode = aes.Decrypt(param.ProtectionCode, param.HashKey);
            var validateResult = ValidateProtectionCode(rawProtectionCode);

            if (validateResult.IsFailed)
            {
                return validateResult;
            }

            var criteria = new UpdateProtectionCodeCriteria(
                encryptor.CfsEnCode(rawProtectionCode),
                param.CustId,
                param.SubAccId,
                param.LangKey,
                param.IP);
            await repository.ExecuteAsync(criteria);
            var result = new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };

            if (result.IsOk)
            {
                result.Message = Resources.UpdateProtectionCodeSuccessfully;
            }

            return result;
        }

        private static Result ValidateProtectionCode(string protectionCode)
        {
            if (protectionCode.Length < 6)
            {
                return Result.CreateErrorResult(Resources.MinLengthNewProtectionCodeError);
            }

            const int Space = 32;
            const int MaxAnsiCode = 127;

            if (protectionCode.Any(c => c == Space || c > MaxAnsiCode))
            {
                return Result.CreateErrorResult(Resources.ContainsSpaceOrUnicodeCharacterError);
            }

            return new Result();
        }
    }
}
