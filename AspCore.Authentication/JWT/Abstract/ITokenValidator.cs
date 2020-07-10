using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Authentication.JWT.Abstract
{
    public interface ITokenValidator<TJWTInfo>
        where TJWTInfo : class, IJWTEntity, new()
    {
        ServiceResult<TJWTInfo> Validate(AuthenticationToken token, bool isExpiredToken);

        bool ValidatePublicKey { get; }
    }
}
