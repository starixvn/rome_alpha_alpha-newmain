namespace Alpha.Main.Services.Impl
{
    using System;
    using Alpha.Main.Criteria.Theme;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions.Web;

    public class ThemeServiceImpl : ThemeService
    {
        private readonly IDynamicRepository repository;

        public ThemeServiceImpl(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public void WriteLog(int custId, int subAccId, bool isDarkMode, string ip, string location)
        {
            try
            {
                repository.Execute(new LogChangeThemeCriteria(custId, subAccId, isDarkMode, ip, location));
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
