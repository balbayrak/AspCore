using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;

namespace AspCore.ApiAuthentication.JWT.Concrete
{
    public static class HttpContextExtension
    {
        public static ServiceResult<TJWTInfo> GetJWTToken<TJWTInfo>(this HttpContext httpContext)
            where TJWTInfo : class, IJWTEntity, new()
        {
            ServiceResult<TJWTInfo> serviceResult = new ServiceResult<TJWTInfo>();
            try
            {
                if (httpContext.Request.Headers.ContainsKey(ApiConstants.Api_Keys.API_AUTHORIZATION) && httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0].StartsWith("Bearer "))
                {
                    string tokenJson = httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0]
                        .Substring("Bearer ".Length);

                    ITokenGenerator<TJWTInfo> tokenGenerator = DependencyResolver.Current.GetService<ITokenGenerator<TJWTInfo>>();

                    serviceResult = tokenGenerator.GetJWTInfo(new Concrete.AuthenticationToken
                    {
                        access_token = tokenJson
                    });
                }
                else
                {
                    serviceResult.ErrorMessage = BusinessConstants.JWT_Error_Messages.BEARER_TOKEN_NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(BusinessConstants.JWT_Error_Messages.BEARER_TOKEN_GET_ERROR, ex);
            }


            return serviceResult;
        }
    }
}
