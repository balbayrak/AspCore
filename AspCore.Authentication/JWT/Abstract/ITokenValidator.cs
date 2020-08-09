using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspCore.Authentication.JWT.Abstract
{
    public interface ITokenValidator<TJWTInfo>
        where TJWTInfo : class, IJWTEntity, new()
    {
        ServiceResult<TJWTInfo> Validate(AuthenticationTicketInfo token, bool isExpiredToken);

        ServiceResult<List<Claim>> GetClaims(AuthenticationTicketInfo token);

        bool ValidatePublicKey { get; }
    }
}
