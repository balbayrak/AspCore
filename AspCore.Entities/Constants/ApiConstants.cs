﻿using System;
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
            public const string UPDATE = "Update";
            public const string DELETE = "Delete";
            public const string DELETE_WITH_IDs = "DeleteWithIDs";
            public const string GET_ALL = "GetAll";
            public const string GET_ALL_ASYNC = "GetAllAsync";
            public const string GET_BY_ID = "GetById";
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

        public struct CacheApi_Urls
        {
            public static readonly string CREATE_ACTION_NAME = "CreateCacheData";
            public static readonly string READ_ACTION_NAME = "ReadCacheData";
            public static readonly string UPDATE_ACTION_NAME = "UpdateCacheData";
            public static readonly string DELETE_ACTION_NAME = "DeleteCacheData";
            public static readonly string GETDATA_ACTION_NAME = "GetCacheData";
            public static readonly string MIN_MAX_ACTION_NAME = "MinMaxCacheData";
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

            public const string CUSTOM_TOKEN_STORAGE_KEY = "CT_A1CC32F1-73A3-47B4-A1D8-7599C8D69BC9";
        }
    }
}
