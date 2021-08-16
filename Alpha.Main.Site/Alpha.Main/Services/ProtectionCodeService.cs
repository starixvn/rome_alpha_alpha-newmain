namespace Alpha.Main.Services
{
    using System.Threading.Tasks;
    using Alpha.Main.Models;
    using Alpha.Main.Params.ProtectionCode;

    public interface ProtectionCodeService
    {
        string GenerateHashKey();

        Task<string> GetAsync(int custId, int subAccId);

        Task<Result> UpdateAsync(UpdateProtectionCodeParam param);
    }
}
