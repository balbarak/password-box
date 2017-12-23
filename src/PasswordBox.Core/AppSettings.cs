using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordBox.Core
{
    public class AppSettings
    {
        public static IConfiguration Configuration { get; set; }
        
    }
}
