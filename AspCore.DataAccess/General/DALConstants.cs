using AspCore.Entities.EntityType;
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

            public const string DAL_ADD_SUCCESS_MESSAGE = "Added successfully";
            public const string DAL_ADD_SUCCESS_MESSAGE_WITH_PARAMETER = "{0}/s is added successfully";

            public const string DAL_UPDATE_SUCCESS_MESSAGE = "Updated successfully";
            public const string DAL_UPDATE_SUCCESS_MESSAGE_WITH_PARAMETER = "{0}/s is updated successfully";

            public const string DAL_DELETE_SUCCESS_MESSAGE = "Deleted successfully";
            public const string DAL_DELETE_SUCCESS_MESSAGE_WITH_PARAMETER = "{0}/s is deleted successfully";

            public const string ENTITY_IS_NOT_IAUTOHISTORY = "Entity is not a IAutoHistory type";
        }
    }
}
