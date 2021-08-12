namespace Alpha.Main.Services.Impl
{
    using System.Threading.Tasks;
    using Alpha.Main.Criteria.ProtectionCode;
    using Alpha.Main.Models;
    using Alpha.Main.Params.ProtectionCode;
    using Fanex.Data.Repository;
    using Sunplus.Agent.Utilities;

    public class ProtectionCodeServiceImpl : ProtectionCodeService
    {
        private readonly AesService aes;
        private readonly Encryptor encryptor;
        private readonly IDynamicRepository repository;

        public ProtectionCodeServiceImpl(
            AesService aes,
            Encryptor encryptor,
            IDynamicRepository repository)
        {
            this.aes = aes;
            this.encryptor = encryptor;
            this.repository = repository;
        }

        public string GenerateHashKey()
            => encryptor.RandomString(4);

        public async Task<Result> UpdateAsync(UpdateProtectionCodeParam param)
        {
            var rawProtectionCode = aes.Decrypt(param.ProtectionCode, param.HashKey);
            var validateResult = ValidateProtectionCode(rawProtectionCode);

            if (validateResult.IsNotOk)
            {
                return validateResult;
            }

            var encodedProtectionCode = encryptor.CfsEnCode(rawProtectionCode);

            var criteria = new UpdateProtectionCodeCriteria(
                encodedProtectionCode,
                param.CustId,
                param.SubAccId,
                param.LangKey,
                param.IP);
            await repository.ExecuteAsync(criteria);

            return new Result { Code = criteria.ErrCode, Message = criteria.ErrMsg };
        }

        private static Result ValidateProtectionCode(string protectionCode)
        {
            if (string.IsNullOrWhiteSpace(protectionCode) || protectionCode.Length < 6)
            {
                return Result.CreateErrorResult("Protection Code is required.");
            }

            return new Result();
        }
    }
}
