namespace AspCore.WebApi.Authentication.General
{
    internal class SecurityConstants
    {
        public struct LICENCE
        {
            public static string PUBLIC_KEY = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAENnwTbytADVkUDh1xRFoo2+aroGNMXW1Ydzz57kyoXPgCVxhJXQ5P798uFCjEF5uR4Ue6OnVI0W+3GMxbAaj8Xg==";
            public static string LICENCE_EXPIRED_ERROR = "Licence time expired! Check the licence time AspCore framework";
            public static string LICENCE_URL_ERROR = "Application url and licence url dont matched! Check the application url!";
            public static string APPLICATION_URL_GET_ERROR = "Application url not found!";
            public static string LICENCE_VALIDATOR_ERROR = "Licence can not validate, please notify system administrator!";
        }
    }
}
