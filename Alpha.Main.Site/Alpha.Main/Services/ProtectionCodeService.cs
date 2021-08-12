namespace Alpha.Main.Services
{
    using System.Threading.Tasks;
    using Alpha.Main.Models;
    using Alpha.Main.Params.ProtectionCode;

    public interface ProtectionCodeService
    {
        string GenerateHashKey();

        Task<Result> UpdateAsync(UpdateProtectionCodeParam param);
    }
}
