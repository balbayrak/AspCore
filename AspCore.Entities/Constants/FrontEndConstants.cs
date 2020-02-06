namespace AspCore.Entities.Constants
{
    public class FrontEndConstants
    {
        public struct STORAGE_CONSTANT
        {
            public const string COOKIE_USER = "AU";
            public const string TOKEN_INFO = "TI";
        }

        public struct ERROR_MESSAGES
        {
            public const string PARAMETER_IS_NULL = "{0} is null";
            public const string AUTHENTICATE_CLIENT_ERROR = "Authentication failed!";
            public const string GET_USER_INFO_ERROR = "Get Active User info failed!";
        }

    }
}
