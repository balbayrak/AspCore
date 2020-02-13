using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.ConfigurationAccess.Configuration
{
    public enum EnumConfigurationAccessorType
    {
        None = 0,
        AppSettingJson = 1,
        RedisRemoteProvider = 2,
        DataBaseRemoteProvider = 3
    }
}
