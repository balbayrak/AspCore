using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.DataAccess.General
{
    internal class DALConstants
    {
        public struct DALErrorMessages
        {
            public const string DAL_CONFIGURATION_SETTING_ERROR_OCCURRED = "Data Katmanında Configuration işleminde Hata Oluştu! Configurasyon bilgileri configuration helper ile yada doğrudan startup dosyasından sağlanmalıdır.";
            public const string DAL_ERROR_OCCURRED = "Data Katmanında Hata Oluştu!";
            public const string DAL_CONFIGURATION_ERROR_OCCURRED = "Data Access Layer configurasyon bilgileri alınamıyor. Configurasyon bilgileri configuration helper ile yada doğrudan startup dosyasından sağlanmalıdır.";
        }
    }
}
