using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.General
{
    public class WebApiConstants
    {
        public struct BaseExceptionMessages
        {
            public const string INTERNAL_SERVER_ERROR_OCCURRED = "Internal Server Error from the custom middleware!";
            public const string PARAMETER_IS_NULL = "{0} is null";
            public const string PARAMETER_IS_GUID_EMPTY = "{0} is guid empty";
            public const string PARAMETER_IS_NULL_OR_EMPTY = "{0} is null or empty";
            public const string MODEL_INVALID = "Invalid model object";
        }
    }
}
