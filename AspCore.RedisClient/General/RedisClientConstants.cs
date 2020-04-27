using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.RedisClient.General
{
    public class RedisClientConstants
    {
        public struct ERROR_MESSAGES
        {
            public static readonly string KEY_NOT_EMPTY = "Key must not empty!";
            public static readonly string ENTITY_ADD_ERROR = "Cache entity added failure!";
            public static readonly string ENTITY_NOT_FOUND = "Entity not found with key! Please check key value!";
        }
    }
}
