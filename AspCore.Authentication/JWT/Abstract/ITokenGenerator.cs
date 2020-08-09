using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Authentication.JWT.Abstract
{
    public interface ITokenGenerator<TJWTInfo>
        where TJWTInfo : class, IJWTEntity, new()
    {
        ServiceResult<AuthenticationTicketInfo> CreateToken(TJWTInfo jwtInfo);

        ServiceResult<AuthenticationTicketInfo> RefreshToken(AuthenticationTicketInfo token);


    }
}
