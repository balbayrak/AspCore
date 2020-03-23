using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AspCore.Entities.Constants;
using AspCore.Entities.User;
using AspCore.Entities.General;
using System;

namespace AspCore.Extension
{
    public static class HttpContextExtension
    {
        public static string GetHeaderValue(this HttpContext httpContext, string headerName)
        {
            if (httpContext.Request.Headers.ContainsKey(headerName))
            {
                return httpContext.Request.Headers[headerName];
            }

            return null;
        }
        public static ServiceResult<TActiveUser> GetActiveUserInfo<TActiveUser>(this HttpContext httpContext)
                  where TActiveUser : class, IActiveUser, new()
        {
            ServiceResult<TActiveUser> result = new ServiceResult<TActiveUser>();
            try
            {
                if (httpContext.Request.Headers.ContainsKey(HttpContextConstant.HEADER_KEY.ACTIVE_USER))
                {
                    string userInfoJson = httpContext.Request.Headers[HttpContextConstant.HEADER_KEY.ACTIVE_USER];
                    result.Result =  JsonConvert.DeserializeObject<TActiveUser>(userInfoJson);
                    result.IsSucceeded = true;
                }
            }
            catch(Exception ex)
            {
                result.ErrorMessage("Active user info not found in headers", ex);
            }

            return null;
        }
        public static string GetJWTToken(this HttpContext httpContext)
        {
            if (httpContext.Request.Headers.ContainsKey(ApiConstants.Api_Keys.API_AUTHORIZATION) && httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0].StartsWith("Bearer "))
            {
                return httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0]
                    .Substring("Bearer ".Length);

            }

            return null;
        }
    }
}
