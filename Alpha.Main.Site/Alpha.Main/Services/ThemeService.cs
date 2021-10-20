namespace Alpha.Main.Services
{
    public interface ThemeService
    {
        void WriteLog(int custId, int subAccId, bool isDarkMode, string ip, string location);
    }
}
