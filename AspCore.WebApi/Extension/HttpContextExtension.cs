using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.JWT.Concrete;
using Microsoft.AspNetCore.Http;
using System;

namespace AspCore.WebApi.Extension
{
    public static class HttpContextExtension
    {
        public static ServiceResult<TJWTInfo> GetJWTInfo<TJWTInfo>(this HttpContext httpContext)
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

                    serviceResult = tokenGenerator.GetJWTInfo(new AuthenticationToken
                    {
                        access_token = tokenJson
                    });
                }
                else
                {
                    serviceResult.ErrorMessage = SecurityConstants.JWT_Error_Messages.BEARER_TOKEN_NOT_FOUND;
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(SecurityConstants.JWT_Error_Messages.BEARER_TOKEN_GET_ERROR, ex);
            }


            return serviceResult;
        }
    }
}
