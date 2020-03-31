﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Authentication.JWT.Concrete
{
    internal class SecurityConstants
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

        public struct LICENCE
        {
            public static string PUBLIC_KEY = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAENnwTbytADVkUDh1xRFoo2+aroGNMXW1Ydzz57kyoXPgCVxhJXQ5P798uFCjEF5uR4Ue6OnVI0W+3GMxbAaj8Xg==";
            public static string LICENCE_EXPIRED_ERROR = "Licence time expired! Check the licence time AspCore framework";
            public static string LICENCE_URL_ERROR = "Application url and licence url dont matched! Check the application url!";
            public static string APPLICATION_URL_GET_ERROR = "Application url not found!";
            public static string LICENCE_VALIDATOR_ERROR = "Licence can not validate, please notify system administrator!";
        }

        public struct JWT_Error_Messages
        {
            public const string BEARER_TOKEN_NOT_FOUND = "Request header içerisinde bearer token bilgisi bulunamadı!";
            public const string BEARER_TOKEN_GET_ERROR = "Request header içerisinde bearer token bilgisi alınırken hata oluştu!";
        }
    }
}