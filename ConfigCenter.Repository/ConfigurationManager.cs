using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigCenter.Repository
{
    public class ConfigurationManager
    {
        public static Microsoft.Extensions.Configuration.IConfiguration Configuration { get; set; }

    }
}
