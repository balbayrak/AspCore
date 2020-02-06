using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using AspCore.Entities.Constants;
using AspCore.Entities.User;

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
        public static ActiveUser GetActiveUserInfo(this HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.Headers.ContainsKey(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID))
                {
                    string userInfoJson = httpContext.Request.Headers[HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID];
                    return JsonConvert.DeserializeObject<ActiveUser>(userInfoJson);
                }
            }
            catch
            {

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
