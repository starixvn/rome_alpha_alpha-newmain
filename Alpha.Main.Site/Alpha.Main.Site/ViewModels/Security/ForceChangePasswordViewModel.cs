using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Alpha.Main.Site.ViewModels.Security
{
    public class ForceChangePasswordViewModel : PasswordViewModel
    {
        public bool IsDefaultAvatar { get; set; }

        public string EmbedImage { get; set; }

        public string Username { get; set; }

        public string AlternateUsername { get; set; }

        public string UsernameShower { get; set; }

        public LanguageViewModel Language { get; set; }
    }
}