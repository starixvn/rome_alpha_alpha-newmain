namespace Alpha.Main.Services
{
    using System.Threading.Tasks;

    public interface FeedbackService
    {
        Task<bool> CheckExistsAsync(int custId);
    }
}
