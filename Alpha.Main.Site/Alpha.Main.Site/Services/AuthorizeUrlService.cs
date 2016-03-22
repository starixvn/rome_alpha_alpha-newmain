namespace Alpha.Main.Site.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface AuthorizeUrlService
    {
        bool CanAccessUrl(string url);
    }
}
