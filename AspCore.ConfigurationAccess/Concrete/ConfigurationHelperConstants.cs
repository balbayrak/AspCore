using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.ConfigurationAccess.Concrete
{
    public class ConfigurationHelperConstants
    {
        public struct ErrorMessages
        {
            public const string CONFIGURATION_HELPER_NOT_FOUND = "Configuration Helper not found! Please check startup file or injection for IConfigurationHelper!";
        }
    }
}
