using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCore.CacheEntityAccess.Configuration;
using AspCore.CacheEntityApi.Authentication;
using AspCore.Entities.Authentication;
using AspCore.Entities.User;
using AspCore.WebApi;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.Providers.Abstract;
using AspCore.WebApi.Security.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.CacheApi.Controllers
{
    public class AccountController : BaseJWTAuthenticationController<IApiAuthenticationProvider<AuthenticationInfo, CacheApiJWTInfo>, ITokenGenerator<CacheApiJWTInfo>, AuthenticationInfo, CacheApiJWTInfo>
    {
        public override string authenticationProviderName => typeof(CacheApiAppSettingAuthProvider).Name;
        public AccountController()
        {

        }
    }
}