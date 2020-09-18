using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.Constants
{
    public class ApiConstants
    {
        public struct Urls
        {
            public const string LIVENESS = "Liveness";
            public const string READINESS = "Readiness";
            public const string ADD = "Add";
            public const string ADDAsync = "AddAsync";
            public const string UPDATE = "Update";
            public const string UPDATEAsync = "UpdateAsync";
            public const string DELETE = "Delete";
            public const string DELETEAsync = "DeleteAsync";
            public const string DELETE_WITH_IDs = "DeleteWithIDs";
            public const string GET_ALL = "GetAll";
            public const string GET_ALL_ASYNC = "GetAllAsync";
            public const string GET_ENTITY_HISTORIES_ASYNC = "GetEntityHistoriesAsync";
            public const string GET_BY_ID = "GetById";
            public const string GET_BY_IDAsync = "GetByIdAsync";
            public const string GET_BY_IDAsyncWithParams = "GetByIdAsync/{id}";

            public const string AUTHENTICATE_CLIENT = "AuthenticateClient";
            public const string REFRESH_TOKEN = "RefreshToken";
            public const string GET_CLIENT_INFO = "GetClientInfo";
            public const string ADDDOCUMENT = "AddDocument";
            public const string UPDATEDOCUMENT = "UpdateDocument";
            public const string DELETEDOCUMENT = "DeleteDocument";
            public const string READDOCUMENT = "ReadDocument";
            public const string VIEWDOCUMENTS = "ViewDocuments";
        }

        public struct DocumentApi_Urls
        {
            public const string CREATE = "CreateDocument";
            public const string UPDATE = "UpdateDocument";
            public const string DELETE = "DeleteDocument";
            public const string READ = "ReadDocument";
            public const string GET_PROJECTS = "Projects";
            public const string GET_PROJECT_FOLDERS = "ProjectFolders";
        }

        public struct DataSearchApi_Urls
        {
            public const string CREATE_ACTION_NAME = "CreateIndexData";
            public const string READ_ACTION_NAME = "ReadIndexData";
            public const string UPDATE_ACTION_NAME = "UpdateIndexData";
            public const string DELETE_ACTION_NAME = "DeleteIndexData";
            public const string GETDATA_ACTION_NAME = "GetIndexData";
            public const string RESET_INDEX_ACTION_NAME = "ResetIndex";
        }

        public struct Api_Keys
        {
            public const string API_CONTENT_TYPE = "Content-Type";

            public const string API_ACCEPT = "Accept";

            public const string API_AUTHORIZATION = "Authorization";

            public const string API_AUTHORIZATION_BEARER = "Bearer";

            public const string API_AUTHORIZATION_BASIC = "Basic";

            public const string JSON_MEDIA_TYPE_QUALITY_HEADER = "application/json";

            public const string DEFLATE_COMPRESSION_STRING_WITH_QUALITY_HEADER = "deflate";

            public const string GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER = "gzip";

            public const string BEARER_TOKEN_GRANTTYPE = "grant_type";

            public const string BEARER_TOKEN_USERNAME = "username";

            public const string BEARER_TOKEN_PASSWORD = "password";

            public const string AUTHENTICATION_TOKEN_ERROR = "Authentication access token bilgisi alınamıyor, authentication bilgilerinizi kontrol ediniz!";

            public const string API_ACCESS_TOKEN = "ApiAccessToken";

            public const string TOKEN_EXPIRED_HEADER = "Token-Expired";

            public const string TOKEN_EXPIRED_HEADER_STR = "Token Expired";

            public const string APP_USER_STORAGE_KEY = "AppUser_A1CC32F1-73A3-47B4-A1D8-7599C8D69BC9";

            public static string ACCESS_TOKEN { get; } = "access_token";

            public static string REFRESH_TOKEN { get; } = "refresh_token";

            public static string EXPIRES { get; } = "expires";
        }
    }
}
