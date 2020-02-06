using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.ConfigurationAccess.Configuration
{
    public enum EnumConfigurationManager
    {
        None = 0,
        AppSettingJson = 1,
        RedisRemoteProvider = 2,
        DataBaseRemoteProvider = 3
    }
}
