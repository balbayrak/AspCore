using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Security.General
{
    public class SecurityConstants
    {
        public struct TOKEN_SETTING_OPTIONS
        {
            public const string TOKEN_SETTING_KEY = "TokenSettingOption";
            public const string TOKEN_SETTING_KEY_IS_NULL_EXCEPTION = "TokenSettingOption bilgileri appSettings içerisinde yer almıyor, kontrol ediniz!";
            public const string TOKEN_CREATE_ERROR = "Token oluşturulurken hata oluştu, sistem yöneticisine haber veriniz!";
            public const string TOKEN_REFRESH_ERROR = "Token yenileme işleminde hata oluştu, sistem yöneticisine haber veriniz!";
            public const string ACTIVE_USER_INFO_NOT_FOUND = "Token içerisinden active user bilgileri alınamadı !";
            public const string REFRESH_TOKEN_IS_INVALID = "Gönderilen refresh token geçerli bir token değil!";
            public const string REFRESH_TOKEN__CREATE_EXCEPTION = "Refresh Token oluşturulurken hata oluştu!";
            public const string TOKEN_CREATE_EXCEPTION = "Token oluşturulurken hata oluştu!";
            public const string AUTHENTICATION_PROVIDER_NOT_FOUND = "Authentication provider bulunamadı, sistem yöneticisine haber veriniz!";
            public const string OPTION_KEY_IS_NULL_EXCEPTION = "Option bilgileri appSettings içerisinde yer almıyor, kontrol ediniz!";
        }
        public struct AUTHORIZATION
        {
            public const string NOT_AUTHORIZE_ACTION = "Action yada girilen input değerleriyle işlem yetkiniz bulunmamaktadır, sistem yöneticisine haber veriniz!";
        }
    }
}
